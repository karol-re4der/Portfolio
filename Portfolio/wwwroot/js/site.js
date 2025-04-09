//Handle single photo display for gallery
const modal = document.getElementById('singlePhotoModal')
if (modal) {
    modal.addEventListener('show.bs.modal', event => {
        const imagePath = event.relatedTarget.getAttribute('data-bs-path')

        const image = modal.querySelector('.modal-body img')

        image.src = imagePath
    })
}