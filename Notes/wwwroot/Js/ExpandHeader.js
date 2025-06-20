window.expandHeader = {
    init: function () {
        const header = document.getElementById('expandingHeader');
        if (!header) {
            console.warn("expandHeader: #expandingHeader not found");
            return;
        }

        let startY = null;

        window.addEventListener('touchstart', (e) => {
            if (window.scrollY === 0) {
                startY = e.touches[0].clientY;
            }
        });

        window.addEventListener('touchmove', (e) => {
            if (startY !== null) {
                const deltaY = e.touches[0].clientY - startY;

                if (deltaY > 0) {

                    const expanded = Math.min(80 + deltaY, 200);
                    header.style.height = `${expanded}px`;


                    header.classList.add('expanded');

                } else if (deltaY < 0) {

                    header.style.height = `${50}px`;
                    header.classList.remove('expanded');
                }
            }
        });
    }
};