define([], function () {
    return function (view) {
        var btn, logBox;

        function log(msg) {
            var ts = new Date().toLocaleTimeString();
            logBox.value += "[" + ts + "] " + msg + "\n";
            logBox.scrollTop = logBox.scrollHeight;
        }

        function onWebSocketMessage(e, msg) {
            // Emby sends { MessageType: "WebSocket", Data: "<jsonstring>" }
            if (msg && msg.MessageType === "WebSocket") {
                try {
                    var data = JSON.parse(msg.Data || "{}");
                    log("✅ Response: " + JSON.stringify(data));
                } catch (ex) {
                    log("⚠️ Raw: " + (msg.Data || ""));
                }
            }
        }

        view.addEventListener("viewshow", function () {
            btn = view.querySelector("#btnPing");
            logBox = view.querySelector("#logBox");

            log("WebSocketTest loaded, ready.");

            // ✅ subscribe to WebSocket channel using Events.on
            Events.on(ApiClient, "message", onWebSocketMessage);

            btn.addEventListener("click", function () {
                log("Sending ping → WebSocket…");

                var payload = {
                    Type: "ping",
                    UserId: ApiClient.getCurrentUserId() || "",
                    Payload: "Hello from dashboard"
                };

                // must stringify to avoid deserialization error on server
                ApiClient.sendMessage("WebSocket", JSON.stringify(payload));
            });
        });

        view.addEventListener("viewdestroy", function () {
            // ✅ clean up listener when leaving page
            Events.off(ApiClient, "message", onWebSocketMessage);
        });
    };
});

