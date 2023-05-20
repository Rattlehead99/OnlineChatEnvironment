var messageBuilder = function () {
    var message = null;
    var header = null;
    var p = null;
    var footer = null;

    return
    {
        createMessage: function z(classList) {
            message = document.createElement("div")
            if (classList === undefined) {
                classList = [];
            }

            for (var i = 0; i < classList.length; i++) {
                message.classList.add(classList[i])
            }

            message.classList.add('message')

            return this;
        },
        withHeader: function a(text) {
            header = document.createElement("header")
            header.appendChild(document.createTextNode(text + ':'))
            return this;
        },
        withParagraph: function b(text) {
            p = document.createElement("p")
            p.appendChild(document.createTextNode(text))
            return this;
        },
        withFooter: function c(text) {
            footer = document.createElement("footer")
            footer.appendChild(document.createTextNode(text))
            return this;
        },
        build: function d() {
            message.appendChild(header);
            message.appendChild(p);
            message.appendChild(footer);
            return message;
        }
    }
}
