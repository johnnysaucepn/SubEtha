
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

    $(document).on("click", ".edbutton:not(.press-and-hold)", function() {
        var bindingName = $(this).attr('data-edbutton');
        console.log("clicked " + bindingName);
        if (connection.isOpen()) {
            connection.sendBindingActivationRequest({ 'BindingName': bindingName });
        }
    });

    $(document).on("pointerdown", ".edbutton.press-and-hold", function () {
        var bindingName = $(this).attr('data-edbutton');
        console.log("down " + bindingName);
        heldBindingName = bindingName;
        if (connection.isOpen()) {
            connection.sendBindingStartActivationRequest({ 'BindingName': bindingName });
        }
    });

    $(document).on("pointerup pointerout", ".edbutton.press-and-hold", function () {
        if (heldBindingName !== "") {
            console.log("up " + heldBindingName);
            if (connection.isOpen()) {
                connection.sendBindingEndActivationRequest({ 'BindingName': heldBindingName });
            }
            heldBindingName = "";
        }
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
            if (bindingIsInList(bindingList, "HyperSuperCombination").found) {
                button.attr("data-edbutton", "HyperSuperCombination");
            } else if (bindingIsInList(bindingList, "Hyperspace").found) {
                button.attr("data-edbutton", "Hyperspace");
            }
        });

        $(".edbutton").each(function (i) {
            var button = $(this);
            var bindingName = button.attr("data-edbutton");
            var bindingMatch = bindingIsInList(bindingList, bindingName);
            if (bindingMatch.found) {
                button.removeClass("disabled");
                button.prop("disabled", false);
                if (bindingMatch.activation === "Hold") {
                    button.addClass("press-and-hold");
                } else {
                    button.removeClass("press-and-hold");
                }

            } else {
                button.addClass("disabled");
                button.prop("disabled", true);
            }
        });

    } else if (parsedMessage.MessageType === "ControlState") {
        var supercruise = parsedMessage.Supercruise === true;
        var srv = parsedMessage.Srv === true;
        var analysis = parsedMessage.HudAnalysisMode === true;
        var fssMode = parsedMessage.FssMode === true;
        var saaMode = parsedMessage.SaaMode === true;

        // open slide 0 if real-space, 1 if supercruise, 2 if SRV
        $("#travel-zone").carousel(srv ? 2 : supercruise ? 1 : 0);

        // open slide 3 if srv, slide 2 if surface analysis, 1 for system scan, 0 for combat otherwise
        $("#mode-zone").carousel(srv ? 3 : saaMode ? 2 : fssMode ? 1 : 0);
    }
}

function bindingIsInList(list, element) {
    if (list === null) return { found: false };
    if (element === null) return { found: false };
    if (list.length === 0) return { found: false };

    for (var i = 0; i < list.length; i++) {
        if (list[i].startsWith(element)) {
            var activation = list[i].endsWith(":Hold") ? "Hold" : "Press";
            return { found: true, activation: activation };
        }
    }

    return { found: false };
}

function onError(evt) {
    writeToScreen("ERROR: " + evt.data);
}

function writeToScreen(message) {
    $("#output").append(message);
    if (!message.endsWith("\n")) $("#output").append("\n");
}

$(init);