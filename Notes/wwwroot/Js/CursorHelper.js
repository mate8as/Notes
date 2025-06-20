
(function () {
    window.GLOBAL = window.GLOBAL || {};
    GLOBAL.DotNetReference = null;

    let lastX = 0;
    let lastY = 0;
    let dirty = false;

    document.addEventListener('mousemove', function (event) {
        lastX = event.clientX;
        lastY = event.clientY;
        dirty = true;
    });

    function sendLoop() {
        if (dirty && GLOBAL.DotNetReference != null) {
            GLOBAL.DotNetReference.invokeMethodAsync('OnCursorMoved', lastX, lastY);
            dirty = false;
        }
        requestAnimationFrame(sendLoop);
    }

    requestAnimationFrame(sendLoop);

    window.addEventListener('resize', function () {
        if (GLOBAL.DotNetReference != null) {
            GLOBAL.DotNetReference.invokeMethodAsync('SetWindowSize', window.innerWidth, window.innerHeight);
        }
    }, true);

    window.CursorHelper = {
        SetDotnetReference: function (pDotNetReference) {
            GLOBAL.DotNetReference = pDotNetReference;
        },
        GetWindowSize: function () {
            return [window.innerWidth, window.innerHeight];
        }
    };
})();