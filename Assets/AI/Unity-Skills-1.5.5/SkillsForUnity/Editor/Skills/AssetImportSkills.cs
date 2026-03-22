using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections.Generic;

namespace UnitySkills
{
    /// <summary>
    /// Asset Import Skills - Reimport, texture/model settings.
    /// </summary>
    public static class AssetImportSkills
    {
        [UnitySkill("asset_reimport", "Force reimport of an asset")]
        public static object AssetReimport(string assetPath)
        {
            if (string.IsNullOrEmpty(assetPath))
                return new { success = false, error = "assetPath is required" };
            if (Validate.SafePath(assetPath, "assetPath") is object pathErr) return pathErr;

            if (!System.IO.File.Exists(assetPath) && !System.IO.Directory.Exists(assetPath))
            {
                // Try as relative path
                var fullPath = System.IO.Path.Combine(Application.dataPath.Replace("/Assets", ""), assetPath);
                if (!System.IO.File.Exists(fullPath) && !System.IO.Directory.Exists(fullPath))
                    return new { success = false, error = $"Asset not found: {assetPath}" };
            }

            // 重新导入前记录资产状态
            var asset = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(assetPath);
            if (asset != null) WorkflowManager.SnapshotObject(asset);

            AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);
            return new { success = true, reimported = assetPath };
        }

        [UnitySkill("asset_reimport_batch", "Reimport multiple assets matching a pattern")]
        public static object AssetReimportBatch(string searchFilter = "*", string folder = "Assets", int limit = 100)
        {
            var guids = AssetDatabase.FindAssets(searchFilter, new[] { folder });
            var reimported = new List<string>();

            foreach (var guid in guids.Take(limit))
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
                reimported.Add(path);
            }

            return new { success = true, count = reimported.Count, assets = reimported };
        }

        [UnitySkill("texture_set_import_settings", "Set texture import settings (maxSize, compression, readable)")]
        public static object TextureSetImportSettings(
            string assetPath,
            int? maxSize = null,              // 32, 64, 128, 256, 512, 1024, 2048, 4096, 8192
            string compression = null,        // None, LowQuality, NormalQuality, HighQuality
            bool? readable = null,
            bool? generateMipMaps = null,
            string textureType = null)        // Default, NormalMap, Sprite, Cursor, Cookie, Lightmap
        {
            var importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;
            if (importer == null)
                return new { success = false, error = $"Not a texture or not found: {assetPath}" };

            // 修改前记录资产状态
            var asset = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(assetPath);
            if (asset != null) WorkflowManager.SnapshotObject(asset);

            bool changed = false;

            if (maxSize.HasValue)
            {
                importer.maxTextureSize = maxSize.Value;
                changed = true;
            }

            if (!string.IsNullOrEmpty(compression))
            {
                switch (compression.ToLower())
                {
                    case "none": importer.textureCompression = TextureImporterCompression.Uncompressed; break;
                    case "lowquality": importer.textureCompression = TextureImporterCompression.CompressedLQ; break;
                    case "normalquality": importer.textureCompression = TextureImporterCompression.Compressed; break;
                    case "highquality": importer.textureCompression = TextureImporterCompression.CompressedHQ; break;
                }
                changed = true;
            }

            if (readable.HasValue)
            {
                importer.isReadable = readable.Value;
                changed = true;
            }

            if (generateMipMaps.HasValue)
            {
                importer.mipmapEnabled = generateMipMaps.Value;
                changed = true;
            }

            if (!string.IsNullOrEmpty(textureType))
            {
                switch (textureType.ToLower())
                {
                    case "default": importer.textureType = TextureImporterType.Default; break;
                    case "normalmap": importer.textureType = TextureImporterType.NormalMap; break;
                    case "sprite": importer.textureType = TextureImporterType.Sprite; break;
                    case "cursor": importer.textureType = TextureImporterType.Cursor; break;
                    case "cookie": importer.textureType = TextureImporterType.Cookie; break;
                    case "lightmap": importer.textureType = TextureImporterType.Lightmap; break;
                }
                changed = true;
            }

            if (changed)
            {
                importer.SaveAndReimport();
            }

            return new
            {
                success = true,
                assetPath,
                maxSize = importer.maxTextureSize,
                compression = importer.textureCompression.ToString(),
                readable = importer.isReadable,
                mipmaps = importer.mipmapEnabled
            };
        }

        [UnitySkill("model_set_import_settings", "Set model (FBX) import settings")]
        public static object ModelSetImportSettings(
            string assetPath,
            float? globalScale = null,
            bool? importMaterials = null,
            bool? importAnimation = null,
            bool? generateColliders = null,
            bool? readable = null,
            string meshCompression = null)    // Off, Low, Medium, High
        {
            var importer = AssetImporter.GetAtPath(assetPath) as ModelImporter;
            if (importer == null)
                return new { success = false, error = $"Not a model or not found: {assetPath}" };

            // 修改前记录资产状态
            var asset = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(assetPath);
            if (asset != null) WorkflowManager.SnapshotObject(asset);

            bool changed = false;

            if (globalScale.HasValue)
            {
                importer.globalScale = globalScale.Value;
                changed = true;
            }

            if (importMaterials.HasValue)
            {
                importer.materialImportMode = importMaterials.Value 
                    ? ModelImporterMaterialImportMode.ImportViaMaterialDescription 
                    : ModelImporterMaterialImportMode.None;
                changed = true;
            }

            if (importAnimation.HasValue)
            {
                importer.importAnimation = importAnimation.Value;
                changed = true;
            }

            if (generateColliders.HasValue)
            {
                importer.addCollider = generateColliders.Value;
                changed = true;
            }

            if (readable.HasValue)
            {
                importer.isReadable = readable.Value;
                changed = true;
            }

            if (!string.IsNullOrEmpty(meshCompression))
            {
                switch (meshCompression.ToLower())
                {
                    case "off": importer.meshCompression = ModelImporterMeshCompression.Off; break;
                    case "low": importer.meshCompression = ModelImporterMeshCompression.Low; break;
                    case "medium": importer.meshCompression = ModelImporterMeshCompression.Medium; break;
                    case "high": importer.meshCompression = ModelImporterMeshCompression.High; break;
                }
                changed = true;
            }

            if (changed)
            {
                importer.SaveAndReimport();
            }

            return new
            {
                success = true,
                assetPath,
                globalScale = importer.globalScale,
                importAnimation = importer.importAnimation,
                meshCompression = importer.meshCompression.ToString()
            };
        }

        [UnitySkill("audio_set_import_settings", "Set audio clip import settings")]
        public static object AudioSetImportSettings(
            string assetPath, bool? forceToMono = null, bool? loadInBackground = null,
            string loadType = null, string compressionFormat = null, int? quality = null)
        {
            var importer = AssetImporter.GetAtPath(assetPath) as AudioImporter;
            if (importer == null) return new { error = $"Not an audio asset: {assetPath}" };
            var asset = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(assetPath);
            if (asset != null) WorkflowManager.SnapshotObject(asset);
            if (forceToMono.HasValue) importer.forceToMono = forceToMono.Value;
            if (loadInBackground.HasValue) importer.loadInBackground = loadInBackground.Value;
            var settings = importer.defaultSampleSettings;
            if (!string.IsNullOrEmpty(loadType) && System.Enum.TryParse<AudioClipLoadType>(loadType, true, out var lt))
                settings.loadType = lt;
            if (!string.IsNullOrEmpty(compressionFormat) && System.Enum.TryParse<AudioCompressionFormat>(compressionFormat, true, out var cf))
                settings.compressionFormat = cf;
            if (quality.HasValue) settings.quality = quality.Value / 100f;
            importer.defaultSampleSettings = settings;
            importer.SaveAndReimport();
            return new { success = true, assetPath, forceToMono = importer.forceToMono, loadType = settings.loadType.ToString(), compressionFormat = settings.compressionFormat.ToString() };
        }

        [UnitySkill("sprite_set_import_settings", "Set sprite import settings (mode, pivot, packingTag, pixelsPerUnit)")]
        public static object SpriteSetImportSettings(
            string assetPath, string spriteMode = null, float? pixelsPerUnit = null,
            string packingTag = null, string pivotX = null, string pivotY = null)
        {
            var importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;
            if (importer == null) return new { error = $"Not a texture: {assetPath}" };
            var asset = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(assetPath);
            if (asset != null) WorkflowManager.SnapshotObject(asset);
            importer.textureType = TextureImporterType.Sprite;
            if (!string.IsNullOrEmpty(spriteMode))
            {
                switch (spriteMode.ToLower())
                {
                    case "single": importer.spriteImportMode = SpriteImportMode.Single; break;
                    case "multiple": importer.spriteImportMode = SpriteImportMode.Multiple; break;
                    case "polygon": importer.spriteImportMode = SpriteImportMode.Polygon; break;
                }
            }
            if (pixelsPerUnit.HasValue) importer.spritePixelsPerUnit = pixelsPerUnit.Value;
            if (!string.IsNullOrEmpty(packingTag)) importer.spritePackingTag = packingTag;
            if (pivotX != null && pivotY != null)
                importer.spritePivot = new Vector2(float.Parse(pivotX, System.Globalization.CultureInfo.InvariantCulture), float.Parse(pivotY, System.Globalization.CultureInfo.InvariantCulture));
            importer.SaveAndReimport();
            return new { success = true, assetPath, spriteMode = importer.spriteImportMode.ToString(), pixelsPerUnit = importer.spritePixelsPerUnit };
        }

        [UnitySkill("texture_get_import_settings", "Get current texture import settings")]
        public static object TextureGetImportSettings(string assetPath)
        {
            var importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;
            if (importer == null) return new { error = $"Not a texture: {assetPath}" };
            return new
            {
                success = true, assetPath, textureType = importer.textureType.ToString(),
                maxSize = importer.maxTextureSize, compression = importer.textureCompression.ToString(),
                readable = importer.isReadable, mipmaps = importer.mipmapEnabled,
                spriteMode = importer.spriteImportMode.ToString(), pixelsPerUnit = importer.spritePixelsPerUnit
            };
        }

        [UnitySkill("model_get_import_settings", "Get current model import settings")]
        public static object ModelGetImportSettings(string assetPath)
        {
            var importer = AssetImporter.GetAtPath(assetPath) as ModelImporter;
            if (importer == null) return new { error = $"Not a model: {assetPath}" };
            return new
            {
                success = true, assetPath, globalScale = importer.globalScale,
                importAnimation = importer.importAnimation, importMaterials = importer.materialImportMode != ModelImporterMaterialImportMode.None,
                meshCompression = importer.meshCompression.ToString(), readable = importer.isReadable,
                generateColliders = importer.addCollider
            };
        }

        [UnitySkill("audio_get_import_settings", "Get current audio import settings")]
        public static object AudioGetImportSettings(string assetPath)
        {
            var importer = AssetImporter.GetAtPath(assetPath) as AudioImporter;
            if (importer == null) return new { error = $"Not an audio asset: {assetPath}" };
            var s = importer.defaultSampleSettings;
            return new
            {
                success = true, assetPath, forceToMono = importer.forceToMono,
                loadInBackground = importer.loadInBackground,
                loadType = s.loadType.ToString(), compressionFormat = s.compressionFormat.ToString(),
                quality = s.quality
            };
        }

        [UnitySkill("asset_set_labels", "Set labels on an asset")]
        public static object AssetSetLabels(string assetPath, string labels)
        {
            var asset = AssetDatabase.LoadMainAssetAtPath(assetPath);
            if (asset == null) return new { error = $"Asset not found: {assetPath}" };
            var labelArray = labels.Split(',').Select(l => l.Trim()).Where(l => l.Length > 0).ToArray();
            AssetDatabase.SetLabels(asset, labelArray);
            return new { success = true, assetPath, labels = labelArray };
        }

        [UnitySkill("asset_get_labels", "Get labels of an asset")]
        public static object AssetGetLabels(string assetPath)
        {
            var asset = AssetDatabase.LoadMainAssetAtPath(assetPath);
            if (asset == null) return new { error = $"Asset not found: {assetPath}" };
            var labels = AssetDatabase.GetLabels(asset);
            return new { success = true, assetPath, labels };
        }
    }
}
