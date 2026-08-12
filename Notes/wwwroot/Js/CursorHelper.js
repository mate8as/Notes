
(function () {
    window.GLOBAL = window.GLOBAL || {};
    GLOBAL.DotNetReference = null;

    let lastX = 0;
    let lastY = 0;
    let dirty = false;
    let sendInFlight = false;
    let lastSentAt = 0;
    const sendInterval = 1000 / 30;

    document.addEventListener('mousemove', function (event) {
        lastX = event.clientX;
        lastY = event.clientY;
        dirty = true;
    });

    function sendLoop(timestamp) {
        if (dirty && !sendInFlight && GLOBAL.DotNetReference != null && timestamp - lastSentAt >= sendInterval) {
            const dotNetReference = GLOBAL.DotNetReference;
            const x = lastX;
            const y = lastY;

            dirty = false;
            sendInFlight = true;
            lastSentAt = timestamp;

            dotNetReference.invokeMethodAsync('OnCursorMoved', x, y)
                .catch(function () {
                    // The component may have been disposed during navigation.
                })
                .finally(function () {
                    sendInFlight = false;
                });
        }
        requestAnimationFrame(sendLoop);
    }

    requestAnimationFrame(sendLoop);

    window.CursorHelper = {
        SetDotnetReference: function (pDotNetReference) {
            GLOBAL.DotNetReference = pDotNetReference;
        },
        ClearDotnetReference: function () {
            GLOBAL.DotNetReference = null;
        }
    };
})();
