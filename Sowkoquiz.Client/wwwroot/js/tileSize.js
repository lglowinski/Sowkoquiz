function calculatePageSize() {
    const containerHeight = window.innerHeight - 200;
    const cardHeight = calculateCardHeight();

    return Math.floor(containerHeight / cardHeight) * 4;
}

function calculateCardHeight() {
    switch (window.innerWidth) {
        case window.innerWidth < 640:
            return 50;
        case window.innerWidth < 768:
            return 100;
        case window.innerWidth < 1024:
            return 200;
        default:
            return 200;
    }
}