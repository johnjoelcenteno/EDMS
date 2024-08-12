window.telerikClientExporter = {
    // A simple adaptor pattern can surface Kendo APIs to Blazor
    saveAs: KendoFileSaver.saveAs,
    exportImage: function (elementRef, options) {
        return KendoDrawing.drawDOM(elementRef, options)
            .then((g) => KendoDrawing.exportImage(g));
    },
    exportPDF: function (elementRef, options) {
        return KendoDrawing.drawDOM(elementRef, options)
            .then((g) => KendoDrawing.exportPDF(g));
    },
    resizeGraph: function () {
        var timeout = false;
        window.addEventListener("resize", function () {
            if (timeout !== false)
                clearTimeout(timeout);
            timeout = setTimeout(raiseResizeEvent, 200);
        });
    },
    countPageLegalElements: function () {
        document.getElementsByClassName('totalPageSpan').innerText = document.querySelectorAll('.page-legal').length;
    },
    removeDivWrap: function () {
        console.log("removeDivWrap called");
        var pageLegal = document.getElementsByClassName("page-legal");
        var pageLegalArray = Array.from(pageLegal).slice(1);

        var divs = document.getElementsByClassName("remove");
        var divsArray = Array.from(divs);

        pageLegalArray.forEach(page => {
            divsArray.forEach(div => {
                var unwrappedContent = unwrapDiv(div);
                unwrappedContent.forEach(tab => {
                    page.innerHTML += tab.outerHTML; // Correcting the property name to outerHTML
                });
            });
        });

        var containerArray = Array.from(document.getElementsByClassName("container")[0].children);
        containerArray.forEach(del => {
            if (del.tagName == "TABLE") {
                del.parentNode.removeChild(del);
            }
        })

    }
}

function b64toBlob(b64Data, contentType) {
    contentType = contentType || '';
    var sliceSize = 512;
    var byteCharacters = atob(b64Data);
    var byteArrays = [];

    for (var offset = 0; offset < byteCharacters.length; offset += sliceSize) {
        var slice = byteCharacters.slice(offset, offset + sliceSize);

        var byteNumbers = new Array(slice.length);
        for (var i = 0; i < slice.length; i++) {
            byteNumbers[i] = slice.charCodeAt(i);
        }

        var byteArray = new Uint8Array(byteNumbers);
        byteArrays.push(byteArray);
    }

    var blob = new Blob(byteArrays, { type: contentType });
    return blob;
}


window.saveAsFile = (fileName, content) => {
    const blob = b64toBlob(content, 'application/octet-stream');
    const url = window.URL.createObjectURL(blob);
    const a = document.createElement('a');
    a.href = url;
    a.download = fileName;
    document.body.appendChild(a);
    a.click();
    window.URL.revokeObjectURL(url);
};


function raiseResizeEvent() {

    var assemblyName = 'DPWH.EDMS.Web'; // Set this to your Blazor app's assembly name
    var method = 'RaiseWindowResizeEvent';
    DotNet.invokeMethodAsync(assemblyName, method, Math.floor(window.innerWidth), Math.floor(window.innerHeight));

}

function unwrapDiv(div) {
    if (div) {
        // Get the parent node of the element
        var parentElement = div.parentNode;
        var firstDiv = div.querySelectorAll("table");
        var divsArray = [...firstDiv];
        // Move all children of the element to its parent node
        while (div.firstChild) {
            parentElement.insertBefore(div.firstChild, div);
        }

        parentElement.removeChild(div);
        return divsArray;
    }
}