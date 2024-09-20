import React, { useState } from 'react';
import axios from 'axios';
import { FaFileUpload } from "react-icons/fa";
import { FiSave } from "react-icons/fi";
import { saveAs } from 'file-saver';

const App: React.FC = () => {
  const [file, setFile] = useState<File | null>(null);
  const [xmlPreview, setXmlPreview] = useState<string | null>(null);
  const [nodes, setNodes] = useState<string[]>([]);
  const [exportOption, setExportOption] = useState<'new' | 'existing'>('new');
  const [log, setLog] = useState<string[]>([]);
  const [loading, setLoading] = useState<boolean>(false); 
  const [alertMessage, setAlertMessage] = useState<string | null>(null); 
  const [chosenDirectory, setChosenDirectory] = useState<FileSystemDirectoryHandle | null>(null); 
  const [existingFile, setExistingFile] = useState<File | null>(null); 

  
  const handleFileChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    if (e.target.files && e.target.files[0]) {
      const uploadedFile = e.target.files[0];
      setFile(uploadedFile);
      
      const reader = new FileReader();
      reader.onload = () => {
        setXmlPreview(reader.result as string);
        addToLog(`Uploaded file: ${uploadedFile.name}`);
      };
      
      reader.readAsText(uploadedFile);
    }
  };

 
  const handleExistingFileChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    if (e.target.files && e.target.files[0]) {
      const selectedFile = e.target.files[0];
      setExistingFile(selectedFile);
      addToLog(`Selected existing file: ${selectedFile.name}`);
    }
  };

  
  const toggleNode = (node: string) => {
    if (nodes.includes(node)) {
      setNodes(nodes.filter(n => n !== node));
      addToLog(`Deselected node: ${node}`);
    } else {
      setNodes([...nodes, node]);
      addToLog(`Selected node: ${node}`);
    }
  };

  
  const handleExport = async () => {
    if (!file) {
      setAlertMessage("Please upload a file before exporting.");
      return;
    }
  
    setLoading(true); 
    setAlertMessage(null); 
  
    const formData = new FormData();
    formData.append('uploadedFile', file); 
    
    
    if (nodes.length > 0) {
      formData.append('nodesJson', JSON.stringify(nodes)); 
    }
    
    formData.append('exportOption', exportOption);

    
    if (exportOption === 'existing' && existingFile) {
      formData.append('existingFile', existingFile); 
    }
  
    try {
      const response = await axios.post(
        'https://localhost:44382/parsers/import-from-xml', 
        formData,
        {
          headers: {
            'Content-Type': 'multipart/form-data',
          },
          responseType: 'blob',
        }
      );
  
      const url = window.URL.createObjectURL(new Blob([response.data]));
      const link = document.createElement('a');
      link.href = url;
      link.download = `Export_${new Date().toISOString()}.xlsx`;
      document.body.appendChild(link);
      link.click();
      document.body.removeChild(link);
  
      setAlertMessage("Datei erfolgreich exportiert!"); 
    } catch (error) {
      console.error('Export fehlgeschlagen', error);
      setAlertMessage("Export fehlgeschlagen. Bitte versuchen Sie es erneut."); 
    } finally {
      setLoading(false); 
    }
  };


const saveLogFile = () => {
  const logContent = log.join("\n");
  const blob = new Blob([logContent], { type: 'text/plain' });
  const link = document.createElement('a');
  link.href = URL.createObjectURL(blob);
  link.download = 'export_log.txt';
  document.body.appendChild(link);
  link.click();
  document.body.removeChild(link);
};

  
  const addToLog = (activity: string) => {
    setLog(prevLog => [...prevLog, activity]);
  };

 
  const handleDownload = async () => {
    if ('showDirectoryPicker' in window) {
      try {
        const dirHandle = await window.showDirectoryPicker();
        setChosenDirectory(dirHandle);
        addToLog('Directory selected for saving files.');
      } catch (error) {
        console.error('Error selecting directory:', error);
      }
    } else {
      setAlertMessage("Directory picker is not supported in this browser.");
    }
  };

  
  const availableNodes = ["Name", "Alter", "Adresse", "Telefonnummer"];

  return (
    <div className="flex justify-center min-h-screen p-8 bg-gray-100">
    
      <div className="flex w-full max-w-6xl space-x-8">
       
        <div className="w-3/4 space-y-8">
          
          {alertMessage && (
            <div className="bg-red-500 text-white p-2 rounded">
              {alertMessage}
            </div>
          )}

         
          <div className="flex space-x-8 bg-white p-6 rounded-lg shadow-lg">
            <div className="flex-1">
              <h2 className="text-lg font-semibold mb-4">XML-Datei auswählen</h2>
              <input
                type="file"
                accept=".xml"
                onChange={handleFileChange}
                className="block w-full text-sm text-gray-500 file:mr-4 file:py-2 file:px-4 file:rounded-full file:border-0 file:text-sm file:font-semibold file:bg-blue-50 file:text-blue-700 hover:file:bg-blue-100"
              />
              {file && <p className="mt-2 text-sm">Ausgewählte Datei: {file.name}</p>}
            </div>

            
            <div className="flex-1">
              <h2 className="text-lg font-semibold mb-4">Preview XML</h2>
              <div className="p-4 border border-gray-300 h-48 overflow-auto">
                {xmlPreview ? (
                  <pre className="text-sm whitespace-pre-wrap">{xmlPreview}</pre>
                ) : (
                  <p className="text-sm text-gray-500">Keine Datei ausgewählt</p>
                )}
              </div>
            </div>
          </div>

          
          <div className="bg-white p-6 rounded-lg shadow-lg">
            <h2 className="text-lg font-semibold mb-4"> Nodes auswählen (optional)</h2>
            <div className="space-y-2">
              {availableNodes.map(node => (
                <label key={node} className="block">
                  <input
                    type="checkbox"
                    checked={nodes.includes(node)}
                    onChange={() => toggleNode(node)}
                    className="mr-2"
                  />
                  {node}
                </label>
              ))}
            </div>
          </div>

       
          <div className="flex space-x-8">
           
            <div className="flex-1 bg-white p-6 rounded-lg shadow-lg">
              <h2 className="text-lg font-semibold mb-4">Exportoptionen</h2>
              <label className="block">
                <input
                  type="radio"
                  value="new"
                  checked={exportOption === 'new'}
                  onChange={() => {
                    setExportOption('new');
                    addToLog('Chose to export as a new Excel file');
                    setExistingFile(null); 
                  }}
                  className="mr-2"
                />
                Neue Excel-Datei erstellen
              </label>
              <label className="block mt-2">
                <input
                  type="radio"
                  value="existing"
                  checked={exportOption === 'existing'}
                  onChange={() => {
                    setExportOption('existing');
                    addToLog('Chose to export to an existing Excel file');
                  }}
                  className="mr-2"
                />
              Zu vorhandener Excel-Datei hinzufügen
              </label>

             
              {exportOption === 'existing' && (
                <div className="mt-4">
                  <input
                    type="file"
                    accept=".xlsx"
                    onChange={handleExistingFileChange}
                    className="block w-full text-sm text-gray-500 file:mr-4 file:py-2 file:px-4 file:rounded-full file:border-0 file:text-sm file:font-semibold file:bg-blue-50 file:text-blue-700 hover:file:bg-blue-100"
                  />
                  {existingFile && <p className="mt-2 text-sm">Ausgewählte vorhandene Datei: {existingFile.name}</p>}
                </div>
              )}
            </div>

            
            <div className="flex-1 bg-white p-6 rounded-lg shadow-lg">
              <h2 className="text-lg font-semibold mb-4">Standort speichern</h2>
              <button
                onClick={handleDownload}
                className="block w-full bg-blue-500 text-white font-semibold py-2 rounded hover:bg-blue-600 transition"
              >
                Speicherort auswählen
              </button>
              {chosenDirectory && <p className="mt-2 text-sm">Ausgewähltes Verzeichnis!</p>}
            </div>
          </div>

          <div className="bg-white p-6 rounded-lg shadow-lg">
            <button
              onClick={handleExport}
              className={`w-full bg-blue-500 text-white font-semibold py-2 rounded hover:bg-blue-600 transition ${loading ? 'opacity-50 cursor-not-allowed' : ''}`}
              disabled={loading} 
            >
              {loading ? 'Exporting...' : 'Nach Excel exportieren'}
            </button>
          </div>

        </div>

        
        <div className="w-1/4 bg-white p-6 rounded-lg shadow-lg">
          <div className="flex justify-between items-center mb-4">
            <h2 className="text-lg font-semibold">Log</h2>
          </div>
          <div className="h-40 overflow-y-auto border border-gray-300 p-4 text-sm">
            {log.length > 0 ? (
              <ul className="list-disc list-inside">
                {log.map((entry, index) => (
                  <li key={index}>{entry}</li>
                ))}
              </ul>
            ) : (
              <p>Noch keine Aktivitäten protokolliert.</p>
            )}
          </div>
          <button
            onClick={saveLogFile}
            className="bg-gray-500 flex justify-end text-white font-semibold py-2 mt-4 px-4 rounded hover:bg-gray-600 transition"
          >
            Protokoll als TXT speichern
          </button>

        </div>
      </div>
    </div>
  );
};

export default App;
