function refreshPreview(files, previewId) {
    var preview = document.querySelector("#" + previewId);

    if (preview) {
        if (files.length == 1) {
            var src = URL.createObjectURL(files[0]);
            preview.src = src;
        }
    }
}

function refreshPreviews(files, container, sample) {
    if (files.length > 0) {
        var masonry = Masonry.data(container);

        if (masonry) {
            if (!sample.hidden) {
                sample.hidden = true;
                masonry.remove(sample);
            }

            for (let i = 0; i < files.length; i++) {
                var newNode = sample.cloneNode(true);
                newNode.hidden = false;
                var newId = 'preview-' + (container.children.length+1);
                newNode.id = newId;

                container.append(newNode);

                let src = URL.createObjectURL(files[i]);
                document.querySelector("#" + newId + " img").src = src;
                masonry.appended(newNode);
            }
            $('#'+container.id).imagesLoaded().progress(
                function () {
                    masonry.layout();
                }
            );
        }
    }
}

function resetPreview(previewId) {
    var preview = document.querySelector("#" + previewId);

    if (preview) {
        preview.src = '';
    }
}

function setPreview(path, previewId) {
    var preview = document.querySelector("#" + previewId);

    if (preview) {
        preview.src = path;
    }
}