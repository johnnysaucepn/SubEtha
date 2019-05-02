
var wsUri = "ws://" + window.location.hostname + ":5984/Assistant";

var AssistantConnection = function (uri) {
    this.uri = uri;
    this.websocket = null;
    this.isOpen = function () {
        return this.websocket != null && this.websocket.readyState == this.websocket.OPEN;
    }

    this.open = function () {
        this.websocket = new WebSocket(this.uri);
        this.websocket.onopen = function (evt) { onOpen(evt); }
        this.websocket.onclose = function (evt) { onClose(evt); }
        this.websocket.onmessage = function (evt) { onMessage(evt); }
        this.websocket.onerror = function (evt) { onError(evt); }
    }

    this.close = function () {
        this.websocket.close();
    }

    this.sendKeyBindingRequest = function (message) {
        if (!this.isOpen) return;
        var wrappedMessage = { MessageType: 'ActivateBinding', MessageContent: message };
        var stringifiedMessage = JSON.stringify(wrappedMessage);

        writeToScreen("SENT: " + stringifiedMessage);
        this.websocket.send(stringifiedMessage);
    }

    this.sendAvailableBindingsRequest = function (message) {
        if (!this.isOpen) return;

        var wrappedMessage = { MessageType: 'GetAvailableBindings' };
        var stringifiedMessage = JSON.stringify(wrappedMessage);

        writeToScreen("SENT: " + stringifiedMessage);
        this.websocket.send(stringifiedMessage);
    }
}

var connection = new AssistantConnection(wsUri);

function init() {
    connection.open();

    $(".edbutton").each(function(i) {
        $(this).click(function() {
            var bindingName = $(this).attr('data-edbutton');
            console.log("clicked " + bindingName);
            if (connection.isOpen()) {
                connection.sendKeyBindingRequest({ 'BindingName': bindingName });
            }
        });
    });

    $(window).focus(function () {
        if (!connection.isOpen()) {
            connection.open();
        }
    })

    $("#disconnect").click(function() { connection.close(); });
}

function onOpen(evt) {
    writeToScreen("CONNECTED");
    connection.sendAvailableBindingsRequest();
}

function onClose(evt) {
    writeToScreen("DISCONNECTED");
}

function onMessage(evt) {
    writeToScreen('RESPONSE: ' + evt.data + '');
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
    writeToScreen('ERROR: ' + evt.data);
}



function writeToScreen(message) {
    var pre = $("<pre/>").text(message);
    $("#output").append(pre);
}

$(init);