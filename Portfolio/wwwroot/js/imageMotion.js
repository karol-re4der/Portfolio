

window.setInterval(updateImagesInMotion, 1000/30);

function updateImagesInMotion() {
    var elements = document.getElementsByClassName('verticalMotionImage');

    for (let i = 0; i < elements.length; i++ ) {
        var pos = Number(elements[i].style.backgroundPositionY.replace('%', ''));
        var newPos = (pos + 0.01);
        if (newPos > 100) {
            elements[i].classList.add('reversedVerticalMotionImage');
            elements[i].classList.remove('verticalMotionImage');
        }
        else {
            elements[i].style.backgroundPositionY = newPos +'%';
        }
    }

    elements = document.getElementsByClassName('reversedVerticalMotionImage');

    for (let i = 0; i < elements.length; i++) {
        var pos = Number(elements[i].style.backgroundPositionY.replace('%', ''));
        var newPos = (pos - 0.01);
        if (newPos < 0) {
            elements[i].classList.add('verticalMotionImage');
            elements[i].classList.remove('reversedVerticalMotionImage');
        }
        else {
            elements[i].style.backgroundPositionY = newPos +'%';
        }
    }
}