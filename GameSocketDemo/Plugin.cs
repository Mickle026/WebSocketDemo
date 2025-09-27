using GameStreamEngine.WebSocket;
using MediaBrowser.Common;
using MediaBrowser.Common.Configuration;
using MediaBrowser.Common.Plugins;
using MediaBrowser.Controller.Net;
using MediaBrowser.Controller.Session;
using MediaBrowser.Model.Drawing;
using MediaBrowser.Model.Logging;
using MediaBrowser.Model.Plugins;
using MediaBrowser.Model.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.WebSockets;
using System.Xml.Serialization;

namespace WebSocketDemo
{
    public class Plugin : BasePlugin<PluginConfiguration>, IHasWebPages, IHasThumbImage
    {
        public static Plugin Instance { get; private set; }

        public Plugin(IApplicationPaths appPaths,
                      IXmlSerializer xmlSerializer,
                      IJsonSerializer json,
                      ILogger logger)
            : base(appPaths, xmlSerializer)
        {
            Instance = this;

            var handler = new WebSocket.WebSocketHandler(json, logger);
            // No manual registration is needed if Emby auto-discovers IWebSocketListener.
        }

        public override string Name => "Web Socket Demo";
        public override Guid Id => Guid.Parse("A2E23E55-6E17-4CCD-B406-6B948C136B35");
        // --- Thumbnail icon ---
        public Stream GetThumbImage()
        {
            var type = this.GetType();
            return type.Assembly.GetManifestResourceStream(type.Namespace + ".icon.png");
        }

        public ImageFormat ThumbImageFormat => ImageFormat.Png;

        public IEnumerable<PluginPageInfo> GetPages()
        {
            return new[]
            {new PluginPageInfo
                {
                    Name = "WebSocketTest",
                    EmbeddedResourcePath = GetType().Namespace + ".WebUI.WebSocketTest.html"
                },
                new PluginPageInfo
                {
                    Name = "WebSocketTestjs",
                    EmbeddedResourcePath = GetType().Namespace + ".WebUI.WebSocketTest.js"
                }

            };
        }
    }
}

