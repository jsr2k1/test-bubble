//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Abrir textura difuso y baked, añadir la textura baked en un nuevo canal alpha del difuso
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	//Enable double clicking from the Macintosh Finder or the Windows Explorer
	#target photoshop
	//NOTA: ¡¡No poner comentarios después de #target photoshop!!
	
	//GLOBALS
	var inputFolder	//Directorio con las texturas
	var logStream = File("/c/CombinarDiffuseBaked.log")
	var win = new Window ("dialog{}");
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// in case we double clicked the file
	app.bringToFront()
	// Set Adobe Photoshop CS3 to display no dialogs
	app.displayDialogs = DialogModes.NO
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	function SnpCreateProgressBar () 
	{
		this.windowRef = null
	}
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	SnpCreateProgressBar.prototype.run = function()
	{
		//this.windowRef = win;
		
		// Add a panel to contain the components
		win.pnl = win.add("panel", [10, 10, 440, 100], "Combinar difuso y baked");	
		
		// Add a progress bar with a label and initial value of 0, max value of 200.
		win.pnl.progBarLabel = win.pnl.add("statictext", [20, 20, 320, 35], "Progress");
		win.pnl.progBar = win.pnl.add("progressbar", [20, 35, 410, 60], 0, 100);	
		
		// Add buttons
		win.goButton = win.add("button", [25, 110, 125, 140], "Start");
		win.doneButton = win.add("button", [310, 110, 410, 140], "Close");
		
		// Define behavior for the "Done" button
		win.doneButton.onClick = function ()
		{
			win.close();
		}
	
		// Define behavior for the "Start" button
		win.goButton.onClick = function ()
		{
			Main()
		}
	
		// Display the window
		var ret = win.show();
	}
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	function Inicializar()
	{
		if(app.documents.length>0)
		{
			win.close()
			alert("Cierra todos los documentos antes de ejecutar el script.")
			return false
		}
		else
		{
			//Pedir el directorio con las texturas	
			inputFolder = Folder.selectDialog("Select a folder with .DDS")	
			//inputFolder = Folder("/c/test_difuse_baked")
			if(inputFolder == null)
			{
				alert("Carpeta incorrecta")
				return false
			}
			else
			{
				logStream.open('w')
				logStream.write("----------------------------------------------------------------------------\n")	
				logStream.write("Log del script: CombinarDiffuseBaked\n")
				logStream.write("----------------------------------------------------------------------------\n")	
			
				return true
			}
		}
	}
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	function Finalizar()
	{
		logStream.write("----------------------------------------------------------------------------\n")	
		logStream.write("Fin del log\n")
		logStream.write("----------------------------------------------------------------------------\n")	
		
		logStream.close(),
		
		//MessageBox de aviso
		alert("Finalizado.\n Resumen de las operaciones realizadas en:\n c:\\CombinarDiffuseBaked.log")
	}
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Buscar una textura BKD con el prefijo
	function BuscarBaked(prefijo)
	{
		var fileList = inputFolder.getFiles()
		
		for(var i=0; i<fileList.length; i++)
		{
			//Obtenemos el prefijo de la textura difusa
			var nombreFichero = fileList[i].name
			if(nombreFichero.match(prefijo))
			{
				if(nombreFichero.match("BKD"))
				{
					return nombreFichero
				}
			}
		}
	
		return null
	}
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	function saveDDS(nombreDifuso)
	{
		var id74 = charIDToTypeID( "save" );
		var desc17 = new ActionDescriptor();
		var id75 = charIDToTypeID( "As  " );
		var desc18 = new ActionDescriptor();
		var id76 = charIDToTypeID( "barF" );
		desc18.putBoolean( id76, true );
		var id77 = charIDToTypeID( "fdev" );
		desc18.putDouble( id77, 3.000000 );
		var id78 = charIDToTypeID( "fbia" );
		desc18.putDouble( id78, 0.000000 );
		var id79 = charIDToTypeID( "urad" );
		desc18.putDouble( id79, 5.000000 );
		var id80 = charIDToTypeID( "uamo" );
		desc18.putDouble( id80, 0.500000 );
		var id81 = charIDToTypeID( "uthr" );
		desc18.putDouble( id81, 0.000000 );
		var id82 = charIDToTypeID( "xstf" );
		desc18.putDouble( id82, 1.000000 );
		var id83 = charIDToTypeID( "xthf" );
		desc18.putDouble( id83, 1.000000 );
		var id84 = charIDToTypeID( "qual" );
		desc18.putInteger( id84, 71 );
		var id85 = charIDToTypeID( "erdi" );
		desc18.putBoolean( id85, false );
		var id86 = charIDToTypeID( "erdw" );
		desc18.putInteger( id86, 1 );
		var id87 = charIDToTypeID( "usfa" );
		desc18.putBoolean( id87, false );
		var id88 = charIDToTypeID( "txfm" );
		desc18.putInteger( id88, 3 );
		var id89 = charIDToTypeID( "weig" );
		desc18.putInteger( id89, 0 );
		var id90 = charIDToTypeID( "tmty" );
		desc18.putInteger( id90, 0 );
		var id91 = charIDToTypeID( "mmty" );
		desc18.putInteger( id91, 30 );
		var id92 = charIDToTypeID( "smip" );
		desc18.putInteger( id92, 0 );
		var id93 = charIDToTypeID( "bina" );
		desc18.putBoolean( id93, false );
		var id94 = charIDToTypeID( "prem" );
		desc18.putBoolean( id94, false );
		var id95 = charIDToTypeID( "film" );
		desc18.putBoolean( id95, false );
		var id96 = charIDToTypeID( "alpb" );
		desc18.putBoolean( id96, false );
		var id97 = charIDToTypeID( "bord" );
		desc18.putBoolean( id97, false );
		var id98 = charIDToTypeID( "brdr" );
		desc18.putDouble( id98, 0.000000 );
		var id99 = charIDToTypeID( "brdg" );
		desc18.putDouble( id99, 0.000000 );
		var id100 = charIDToTypeID( "brdb" );
		desc18.putDouble( id100, 0.000000 );
		var id101 = charIDToTypeID( "mmft" );
		desc18.putInteger( id101, 2 );
		var id102 = charIDToTypeID( "fdcl" );
		desc18.putBoolean( id102, false );
		var id103 = charIDToTypeID( "fdaf" );
		desc18.putBoolean( id103, false );
		var id104 = charIDToTypeID( "f2rl" );
		desc18.putDouble( id104, 0.500000 );
		var id105 = charIDToTypeID( "f2gl" );
		desc18.putDouble( id105, 0.500000 );
		var id106 = charIDToTypeID( "f2bl" );
		desc18.putDouble( id106, 0.500000 );
		var id107 = charIDToTypeID( "f2al" );
		desc18.putDouble( id107, 0.500000 );
		var id108 = charIDToTypeID( "fddl" );
		desc18.putInteger( id108, 0 );
		var id109 = charIDToTypeID( "fafm" );
		desc18.putDouble( id109, 0.150000 );
		var id110 = charIDToTypeID( "bafh" );
		desc18.putDouble( id110, 0.000000 );
		var id111 = charIDToTypeID( "dthc" );
		desc18.putBoolean( id111, false );
		var id112 = charIDToTypeID( "dth0" );
		desc18.putBoolean( id112, false );
		var id113 = charIDToTypeID( "smth" );
		desc18.putInteger( id113, 0 );
		var id114 = charIDToTypeID( "filg" );
		desc18.putDouble( id114, 2.200000 );
		var id115 = charIDToTypeID( "fieg" );
		desc18.putBoolean( id115, false );
		var id116 = charIDToTypeID( "filw" );
		desc18.putDouble( id116, 10.000000 );
		var id117 = charIDToTypeID( "over" );
		desc18.putBoolean( id117, false );
		var id118 = charIDToTypeID( "fblr" );
		desc18.putDouble( id118, 1.000000 );
		var id119 = charIDToTypeID( "nmcv" );
		desc18.putBoolean( id119, false );
		var id120 = charIDToTypeID( "ncnv" );
		desc18.putInteger( id120, 1009 );
		var id121 = charIDToTypeID( "nflt" );
		desc18.putInteger( id121, 1040 );
		var id122 = charIDToTypeID( "nmal" );
		desc18.putInteger( id122, 1034 );
		var id123 = charIDToTypeID( "nmbr" );
		desc18.putBoolean( id123, false );
		var id124 = charIDToTypeID( "nmix" );
		desc18.putBoolean( id124, false );
		var id125 = charIDToTypeID( "nmiy" );
		desc18.putBoolean( id125, false );
		var id126 = charIDToTypeID( "nmiz" );
		desc18.putBoolean( id126, false );
		var id127 = charIDToTypeID( "nmah" );
		desc18.putBoolean( id127, false );
		var id128 = charIDToTypeID( "nswp" );
		desc18.putBoolean( id128, false );
		var id129 = charIDToTypeID( "nmsc" );
		desc18.putDouble( id129, 2.200000 );
		var id130 = charIDToTypeID( "nmnz" );
		desc18.putInteger( id130, 0 );
		var id131 = charIDToTypeID( "usbi" );
		desc18.putBoolean( id131, false );
		var id132 = charIDToTypeID( "lien" );
		desc18.putBoolean( id132, false );
		var id133 = charIDToTypeID( "shdi" );
		desc18.putBoolean( id133, false );
		var id134 = charIDToTypeID( "shfi" );
		desc18.putBoolean( id134, false );
		var id135 = charIDToTypeID( "shmm" );
		desc18.putBoolean( id135, true );
		var id136 = charIDToTypeID( "shan" );
		desc18.putBoolean( id136, true );
		var id137 = charIDToTypeID( "clrc" );
		desc18.putInteger( id137, 0 );
		var id138 = charIDToTypeID( "vdx1" );
		desc18.putBoolean( id138, true );
		var id139 = charIDToTypeID( "vdx2" );
		desc18.putBoolean( id139, true );
		var id140 = charIDToTypeID( "vdx3" );
		desc18.putBoolean( id140, true );
		var id141 = charIDToTypeID( "vdx5" );
		desc18.putBoolean( id141, true );
		var id142 = charIDToTypeID( "v444" );
		desc18.putBoolean( id142, true );
		var id143 = charIDToTypeID( "v555" );
		desc18.putBoolean( id143, true );
		var id144 = charIDToTypeID( "v565" );
		desc18.putBoolean( id144, true );
		var id145 = charIDToTypeID( "v888" );
		desc18.putBoolean( id145, true );
		var id146 = charIDToTypeID( "alph" );
		desc18.putBoolean( id146, true );
		var id147 = charIDToTypeID( "usra" );
		desc18.putBoolean( id147, false );
		var id148 = charIDToTypeID( "usfs" );
		desc18.putInteger( id148, 0 );
		var id149 = charIDToTypeID( "prev" );
		desc18.putBoolean( id149, false );
		var id150 = charIDToTypeID( "rdep" );
		desc18.putInteger( id150, 3000 );
		var id151 = charIDToTypeID( "lomm" );
		desc18.putBoolean( id151, false );
		var id152 = charIDToTypeID( "scar" );
		desc18.putDouble( id152, 1.000000 );
		var id153 = charIDToTypeID( "scag" );
		desc18.putDouble( id153, 1.000000 );
		var id154 = charIDToTypeID( "scab" );
		desc18.putDouble( id154, 1.000000 );
		var id155 = charIDToTypeID( "scaa" );
		desc18.putDouble( id155, 1.000000 );
		var id156 = charIDToTypeID( "biar" );
		desc18.putDouble( id156, 0.000000 );
		var id157 = charIDToTypeID( "biag" );
		desc18.putDouble( id157, 0.000000 );
		var id158 = charIDToTypeID( "biab" );
		desc18.putDouble( id158, 0.000000 );
		var id159 = charIDToTypeID( "biaa" );
		desc18.putDouble( id159, 0.000000 );
		var id160 = charIDToTypeID( "siar" );
		desc18.putDouble( id160, 1.000000 );
		var id161 = charIDToTypeID( "siag" );
		desc18.putDouble( id161, 1.000000 );
		var id162 = charIDToTypeID( "siab" );
		desc18.putDouble( id162, 1.000000 );
		var id163 = charIDToTypeID( "siaa" );
		desc18.putDouble( id163, 1.000000 );
		var id164 = charIDToTypeID( "biir" );
		desc18.putDouble( id164, 0.000000 );
		var id165 = charIDToTypeID( "biig" );
		desc18.putDouble( id165, 0.000000 );
		var id166 = charIDToTypeID( "biib" );
		desc18.putDouble( id166, 0.000000 );
		var id167 = charIDToTypeID( "biia" );
		desc18.putDouble( id167, 0.000000 );
		var id168 = charIDToTypeID( "outw" );
		desc18.putBoolean( id168, false );
		var id169 = charIDToTypeID( "clcL" );
		desc18.putBoolean( id169, true );
		var id170 = stringIDToTypeID( "NVIDIA D3D/DDS" );
		desc17.putObject( id75, id170, desc18 );
		var id171 = charIDToTypeID( "In  " );
		
		desc17.putPath( id171, new File(inputFolder + "\\" + nombreDifuso))
		executeAction( id74, desc17, DialogModes.NO )
	}
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	function CombinarTexturas(nombreDifuso, nombreBaked)
	{
		//var ficheroDifuso = new File(app.path.toString() + "/" + strSamplesFolderDirectory + "/" + strSamplesFilenameDune)
		var ficheroDifuso = new File(inputFolder + "\\" + nombreDifuso)
		var ficheroBaked = new File(inputFolder + "\\" + nombreBaked)
	
		//Abrir las texturas
		open(ficheroDifuso)
		open(ficheroBaked)
		refDifuso = app.documents[0]
		refBaked = app.documents[1]
	
		app.activeDocument = refDifuso	
		
		//Eliminar el canal alpha si ya tenía uno
		if(refDifuso.channels.length == 4)
		{
			refDifuso.channels[3].remove()
		}
	
		//Añadir un canal alfa a la textura Diffuse	
		var canalAlfa = refDifuso.channels.add()
	
		//Reescalar, seleccionar y copiar la textura Baked
		app.activeDocument = refBaked
		refBaked.resizeImage(refDifuso.width, refDifuso.height, refDifuso.resolution, ResampleMethod.BICUBIC)
		var ancho = refBaked.width
		var alto = refBaked.height
		refBaked.selection.select(Array (Array(0, 0), Array(ancho, 0), Array(ancho, alto), Array(0, alto)), SelectionType.REPLACE, 0, false)
		refBaked.selection.copy()
	
		//Pegar el baked en el canal alfa
		app.activeDocument = refDifuso
		refDifuso.paste()
		
		//Cerrar la textura Baked
		app.activeDocument = refBaked
		app.activeDocument.close(SaveOptions.DONOTSAVECHANGES)
		
		//Guardar la textura resultante	
		app.activeDocument = refDifuso
		saveDDS(nombreDifuso)
		logStream.write("Fichero combinado correctamente: " + nombreDifuso + "\n")
		app.activeDocument.close(SaveOptions.DONOTSAVECHANGES)
	}
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	function Main()
	{	
		if(Inicializar())
		{
			//Inicializar la barra de progreso
			win.pnl.progBar.value=0
			
			var fileList = inputFolder.getFiles()
	
			//Recorremos todas las texturas de la carpeta
			for(var i=0; i<fileList.length; i++)
			{
				var nombreDifuso = fileList[i].name
				var pos = nombreDifuso.indexOf("F_01_PB_DIF")
				
				if(pos != -1)
				{
					$.writeln("Tratando:" + nombreDifuso)				
					//Obtenemos el prefijo de la textura difusa				
					var prefijo = nombreDifuso.substring(0, pos-1)
					nombreBaked=BuscarBaked(prefijo)
	
					if(nombreBaked!=null)
					{
						CombinarTexturas(nombreDifuso, nombreBaked)
					}
					else
					{
						logStream.write("No se encuentra el baked de: " + nombreDifuso + "\n")
					}
				}
				
				//Actualizar la barra de progreso
				win.pnl.progBar.value = 100.0 * (i/(fileList.length-1))
			}
		
			Finalizar()
		}
	}
	
	//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//MAIN
	
	if(typeof(SnpCreateProgressBar_unitTest) == "undefined")
	{
	    new SnpCreateProgressBar().run();
	}
	
	
	
