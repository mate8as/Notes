
var GLOBAL = {};
GLOBAL.DotNetReference = null;
function setDotnetReference(pDotNetReference) {
    GLOBAL.DotNetReference = pDotNetReference;
}

document.addEventListener('mousemove', function (event) {
    console.log('Mouse X:', event.clientX, 'Mouse Y:', event.clientY);

    GLOBAL.DotNetReference.invokeMethodAsync('OnCursorMoved', event.clientX, event.clientY);
});