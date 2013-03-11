﻿var sp = require("SharePoint");

var domainModel = {
    //Read the files in the specified path and return an array of JSON objects.
        var uploadsFolder = sp.currentContext.web.getFolderByServerRelativeUrl(targetPath);

        var result = [];

        //Read the folders from the uploads folder and transform into model that KendoUI expects.
        uploadsFolder.getSubFolders().forEach(function(folder) {
            if (folder.name == "Forms" || folder.name == "_t" || folder.name == "_w")
                return;
            result.push({
                "name": folder.name,
                "type": "d",
            });
        });

        //Read the files from the uploads folder and transform into a model that KendoUI expects;
        uploadsFolder.getFiles().forEach(function(file) {
            result.push({
                "name": file.name,
                "type": "f",
                "size": file.length,
            });
        });

        return result;
    },
    uploadFileToPath: function(targetPath, file) {
        var uploadsFolder = sp.currentContext.web.getFolderByServerRelativeUrl(targetPath);
        var uploadedFile = uploadsFolder.addFile(file, true);

        var result = {
            "name": uploadedFile.name,
            "type": "f",
            "size": uploadedFile.length
        };

        return result;
    },
    createFolder: function(targetPath, newFolderName) {
        var uploadsFolder = sp.currentContext.web.getFolderByServerRelativeUrl(targetPath);

        var newFolder = uploadsFolder.addSubFolder(newFolderName);
        var result = {
            "name": newFolder.name,
            "type": "d",
        };
        return result;
    },
    deleteFile: function(targetPath, fileName) {
        var uploadsFolder = sp.currentContext.web.getFolderByServerRelativeUrl(targetPath);

        //Iterate through the files in the folder until we find a match.
        //TODO: This is a common pattern and could be made easier/more efficient with a Barista function to do the same.
        var file = Enumerable.From(uploadsFolder.getFiles())
            .Where(function(f) { return f.name == fileName; })
            .FirstOrDefault();

        var result = false;

        //If we found a file, delete it.
        if (file != null) {
            var fileParent = file.getParentWeb();

            fileParent.allowUnsafeUpdates = true;
            file.delete();
            fileParent.allowUnsafeUpdates = false;
            result = true;
        }

        return result;
    },
    deleteFolder: function(path, folderName) {
        var uploadsFolder = sp.currentContext.web.getFolderByServerRelativeUrl(path);

        //Iterate through the subfolders in the folder until we find a match.
        //TODO: This is a common pattern and could be made easier/more efficient with a Barista function to do the same.
        var folder = Enumerable.From(uploadsFolder.getSubFolders())
            .Where(function(f) { return f.name == folderName; })
            .FirstOrDefault();

        var result = false;

        //If we found a folder, delete it
        //TODO: implement folder.getParentWeb
        if (folder != null) {
            var folderParent = web;

            folderParent.allowUnsafeUpdates = true;
            folder.delete();
            folderParent.allowUnsafeUpdates = false;
            result = true;
        }

        return result;
    }
};
    case "read":
            result = domainModel.deleteFile(uploadsFolderPath, fileOrFolderNameToDelete);
        }
            result = domainModel.deleteFolder(uploadsFolderPath, fileOrFolderNameToDelete);
        }
}