
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

    this.sendBindingActivationRequest = function (message) {
        if (!this.isOpen) return;
        var wrappedMessage = message;
        wrappedMessage.MessageType = 'ActivateBinding';
        var stringifiedMessage = JSON.stringify(wrappedMessage);

        writeToScreen("SENT: " + stringifiedMessage);
        this.websocket.send(stringifiedMessage);
    }

    this.sendBindingStartActivationRequest = function (message) {
        if (!this.isOpen) return;
        var wrappedMessage = message;
        wrappedMessage.MessageType = 'StartActivateBinding';
        var stringifiedMessage = JSON.stringify(wrappedMessage);

        writeToScreen("SENT: " + stringifiedMessage);
        this.websocket.send(stringifiedMessage);
    }

    this.sendBindingEndActivationRequest = function (message) {
        if (!this.isOpen) return;
        var wrappedMessage = message;
        wrappedMessage.MessageType = 'EndActivateBinding';
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
var heldBindingName = "";

function init() {

    connection.open();

    $(".edbutton:not(.press-and-hold)").each(function(i) {
        $(this).on("click", function() {
            var bindingName = $(this).attr('data-edbutton');
            console.log("clicked " + bindingName);
            if (connection.isOpen()) {
                connection.sendBindingActivationRequest({ 'BindingName': bindingName });
            }
        });
    });

    $(".edbutton.press-and-hold").each(function (i) {
        $(this).on("pointerdown", function () {
            var bindingName = $(this).attr('data-edbutton');
            console.log("down " + bindingName);
            heldBindingName = bindingName;
            if (connection.isOpen()) {
                connection.sendBindingStartActivationRequest({ 'BindingName': bindingName });
            }
        });
        $(this).on("pointerup pointerout", function () {
            if (heldBindingName !== "") {
                console.log("up " + heldBindingName);
                if (connection.isOpen()) {
                    connection.sendBindingEndActivationRequest({ 'BindingName': heldBindingName });
                }
                heldBindingName = "";
            }
        });
    });

    $(window).on("focus", function () {
        if (!connection.isOpen()) {
            connection.open();
        }
    });

    $(window).on("unload", function () {
        if (connection.isOpen()) {
            connection.close();
        }
    });

    $("#disconnect").on("click", function () {
        if (connection.isOpen()) {
            connection.close();
        }
    });

    $(".carousel").carousel({ interval: false }).carousel(0);
}

function onOpen(evt) {
    writeToScreen("CONNECTED");
    $(".tab-pane").css("background-color", "");
    connection.sendAvailableBindingsRequest();
}

function onClose(evt) {
    writeToScreen("DISCONNECTED");
    $(".tab-pane").css("background-color", "#600");
}

function onMessage(evt) {
    writeToScreen('RECEIVED: ' + evt.data + '');
    var parsedMessage = JSON.parse(evt.data);

    if (parsedMessage.MessageType === "AvailableBindings") {
        var bindingList = parsedMessage.Bindings;

        // Special case for Jump button: activate HyperSuperCombination if possible, plain Hyperspace otherwise
        // As long as you have one of those, you should be able to hyper-jump.
        $("#meta-jump").each(function (i) {
            var button = $(this);
            if (isInList(bindingList, "HyperSuperCombination")) {
                button.attr("data-edbutton", "HyperSuperCombination");
            } else if (isInList(bindingList, "Hyperspace")) {
                button.attr("data-edbutton", "Hyperspace");
            }
        });

        $(".edbutton").each(function (i) {
            var button = $(this);
            var bindingName = button.attr("data-edbutton");
            if (isInList(bindingList, bindingName)) {
                button.removeClass("disabled");
                button.prop("disabled", false);
            } else {
                button.addClass("disabled");
                button.prop("disabled", true);
            }
        });

    } else if (parsedMessage.MessageType === "ControlState") {
        var supercruise = parsedMessage.Supercruise === true;
        var analysis = parsedMessage.HudAnalysisMode === true;
        var fssMode = parsedMessage.FssMode === true;
        var saaMode = parsedMessage.SaaMode === true;

        // open slide 0 if real-space, 1 if supercruise
        $("#travel-carousel").carousel(supercruise ? 1 : 0);

        // open slide 2 if surface analysis, 1 for system scan, 0 for combat otherwise
        $("#mode-carousel").carousel(saaMode ? 2 : fssMode ? 1 : 0);
    }
}

function isInList(list, element) {
    if (list === null) return false;
    if (element === null) return false;
    if (list.length === 0) return false;

    return list.indexOf(element) >= 0;
}

function onError(evt) {
    writeToScreen("ERROR: " + evt.data);
}

function writeToScreen(message) {
    $("#output").append(message);
    if (!message.endsWith("\n")) $("#output").append("\n");
}

$(init);