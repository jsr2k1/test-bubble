//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Create sprite from frames in several files
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	// Enable double clicking from the Macintosh Finder or the Windows Explorer
	#target photoshop
	
	//GLOBALS
	var inputFolder	//Directorio con las texturas
	
	// in case we double clicked the file
	app.bringToFront();
	
	// suppress all dialogs
	app.displayDialogs = DialogModes.NO;
	
	var strtRulerUnits = app.preferences.rulerUnits;
	var strtTypeUnits = app.preferences.typeUnits;
	app.preferences.rulerUnits = Units.PIXELS;
	app.preferences.typeUnits = TypeUnits.POINTS;
	
	var iSourceSizeW = 116;
	var iSourceSizeH = 116;
	var iRows = 4;
	var iCols = 4;
	
	Main();
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	function Inicializar()
	{
		if(app.documents.length>0){
			alert("Cierra todos los documentos antes de ejecutar el script.")
			return false
		}
		else{
			inputFolder = Folder.selectDialog("Select a folder with .DDS")	
			if(inputFolder == null){
				alert("Carpeta incorrecta")
				return false
			}else{
				return true
			}
		}
	}
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	function Main()
	{	
		if(Inicializar())
		{
			//Create a new document
			var targetDoc = app.documents.add(iSourceSizeW*iCols, iSourceSizeH*iRows, 72, "myDocument", NewDocumentMode.RGB);
			//Get all files from folder
			var fileList = inputFolder.getFiles()
			
			for(var i=0;i<fileList.length;i++)
			{
				var fileName = fileList[i].name
				var currentFile = new File(inputFolder + "\\" + fileName)
				open(currentFile)
				
				var sourceDoc = app.documents[1];
				var theLayer = sourceDoc.artLayers[0];
				app.activeDocument = sourceDoc;
				var layerRef = theLayer.duplicate(targetDoc, ElementPlacement.PLACEATBEGINNING);
				app.activeDocument = targetDoc;
				var x = iSourceSizeW * (i%iCols);
				//var y = (iRows-Math.floor(i/iCols)-1);
				//y = y * iSourceSizeH;
				var y = iSourceSizeH * Math.floor(i/iCols);
				layerRef.translate(x,y);
				
				app.activeDocument = sourceDoc;
				app.activeDocument.close(SaveOptions.DONOTSAVECHANGES);
				app.activeDocument = targetDoc;
			}
			//targetDoc.artLayers[0].remove();
			//app.activeDocument.mergeVisibleLayers();
		}
	}


