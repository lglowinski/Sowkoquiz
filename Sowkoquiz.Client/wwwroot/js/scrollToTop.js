window.registerScrollListener = function () {
    const tableContainer = document.querySelector('.table-container');
    const scrollButton = document.querySelector('.scroll-to-top');

    tableContainer.onscroll = function () {
        if (tableContainer.scrollTop > 100) {
            scrollButton.style.display = "block";
        } else {
            scrollButton.style.display = "none";
        }
    };
};

window.scrollToTop = function () {
    const tableContainer = document.querySelector('.table-container');
    tableContainer.scrollTo({ top: 0, behavior: 'smooth' });
    if (document.body.scrollTop > 100 || document.documentElement.scrollTop > 100) {
    window.scrollTo({ top: 0, behavior: 'smooth' });
    }
};