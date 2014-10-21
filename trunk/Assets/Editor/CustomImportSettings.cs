using UnityEngine;
using UnityEditor;
using System;

public class CustomImportSettings : AssetPostprocessor 
{
	//TEXTURES
	void OnPreprocessTexture()
	{
		TextureImporter textureImporter = assetImporter as TextureImporter;
		
		//if(textureImporter.assetPath.Contains("Interface"))
		{
			textureImporter.textureType = TextureImporterType.Sprite;
			textureImporter.textureFormat = TextureImporterFormat.AutomaticTruecolor;
			textureImporter.generateMipsInLinearSpace = false;
		}
	}
	
	/*
	//MODELS
	void OnPreprocessModel()
	{
		ModelImporter modelImporter = assetImporter as ModelImporter;
		modelImporter.generateSecondaryUV=true;
	}
	*/
}
