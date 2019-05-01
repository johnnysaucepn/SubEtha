
// Original test URI and source of this code
//var wsUri = "wss://echo.websocket.org/";
var wsUri = "ws://" + window.location.hostname + ":5984/Assistant";
var output;
var connected = false;

var websocket;
function init() {
    testWebSocket();

    $(".edbutton").each(function(i) {
        $(this).click(function() {
            var bindingName = $(this).attr('data-edbutton');
            console.log("clicked " + bindingName);
            if (connected) {
                sendKeyBindingRequest({ 'BindingName': bindingName });
            }
        });
    });

    $("#disconnect").click(function() { websocket.close(); });
}

function testWebSocket() {
    websocket = new WebSocket(wsUri);
    websocket.onopen = function (evt) { onOpen(evt) };
    websocket.onclose = function (evt) { onClose(evt) };
    websocket.onmessage = function (evt) { onMessage(evt) };
    websocket.onerror = function (evt) { onError(evt) };
}

function onOpen(evt) {
    connected = true;
    writeToScreen("CONNECTED");
    sendAvailableBindingsRequest();
}

function onClose(evt) {
    connected = false;
    writeToScreen("DISCONNECTED");
}

function onMessage(evt) {
    writeToScreen('<span style="color: blue;">RESPONSE: ' + evt.data + '</span>');
    var parsedMessage = JSON.parse(evt.data);

    if (parsedMessage.MessageType === "AvailableBindings") {
        var bindingList = parsedMessage.MessageContent;
        $(".edbutton").each(function (i) {
            var button = $(this);
            var bindingName = button.attr("data-edbutton");
            if (bindingList.indexOf(bindingName) >= 0) {
                button.removeClass("disabled");
            } else {
                button.addClass("disabled");
            }
        });

    }
}

function onError(evt) {
    writeToScreen('<span style="color: red;">ERROR:</span> ' + evt.data);
}

function sendKeyBindingRequest(message) {
    var wrappedMessage = { MessageType: 'ActivateBinding', MessageContent: message };
    var stringifiedMessage = JSON.stringify(wrappedMessage);

    writeToScreen("SENT: " + stringifiedMessage);
    websocket.send(stringifiedMessage);
}

function sendAvailableBindingsRequest(message) {
    var wrappedMessage = { MessageType: 'GetAvailableBindings' };
    var stringifiedMessage = JSON.stringify(wrappedMessage);

    writeToScreen("SENT: " + stringifiedMessage);
    websocket.send(stringifiedMessage);
}

function writeToScreen(message) {
    var pre = $("<pre/>").text(message).css("wordWrap", "break-word");
    $("#output").append(pre);
}

$(init);