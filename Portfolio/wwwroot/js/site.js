var imageWidth = 0;
var imageHeight = 0;
const modal = document.getElementById('singlePhotoModal')

function resizeModal() {
    if (modal) {
        var windowWidth = window.innerWidth;
        var windowHeight = window.innerHeight;

        if (imageWidth < windowWidth && imageHeight < windowHeight) {
            modal.querySelector('.modal-dialog').style = 'max-width: ' + imageWidth + 'px; max-height: ' + imageHeight + 'px; height:'+imageHeight+'px';
        }
        else{
            modal.querySelector('.modal-dialog').style = 'max-width: ' + (imageWidth / imageHeight) * windowHeight + 'px; max-height: ' + windowHeight + 'px; height:' + windowHeight+'px';
        }
    }
}

if (modal) {
    modal.addEventListener('show.bs.modal', event => {
        const imagePath = event.relatedTarget.getAttribute('data-bs-path')
        imageWidth = event.relatedTarget.getAttribute('data-bs-width')
        imageHeight = event.relatedTarget.getAttribute('data-bs-height')

        resizeModal();

        modal.querySelector('.modal-body img').src = imagePath
    })
}
window.addEventListener('resize', resizeModal);
