﻿var edge = require("edge");

var SPContext = function (requestContext) {
};

var SharePoint = (function () {
    function SharePoint(baristaContext) {
        if (!baristaContext)
            throw "Barista Context must be defined.";

        this.m_baristaContext = baristaContext;

        this.__baristaSharePointReferences = [
            this.m_baristaContext.environment.sharePointAssembly,
			this.m_baristaContext.environment.baristaAssembly,
			this.m_baristaContext.environment.baristaSharePointAssembly
        ];

        this.__loadFileAsString = edge.func({
            source: function () {/*

			using Barista.SharePoint;
			using System.Threading.Tasks;

			public class Startup
			{
				public async Task<object> Invoke(string input)
				{
					string filePath, fileContents;
					bool isHiveFile;
					if (SPHelper.TryGetSPFileAsString(input, out filePath, out fileContents, out isHiveFile))
						return fileContents;
					return null;
				}
			}
		*/},
            references: this.__baristaSharePointReferences
        });
    };

    SharePoint.prototype.loadFileAsString = function (fileUrl) {
        return this.__loadFileAsString(fileUrl, true);
    };

    return SharePoint;
})();

module.exports = function (baristaContext) {
    return new SharePoint(baristaContext);
};