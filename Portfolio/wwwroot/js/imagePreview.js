function refreshPreview(files, preview){
    if (files.length > 0)
    {
        let src = URL.createObjectURL(files[0]);
        preview.src = src;
    }
    else
    {
        resetPreview(preview);
    }
}

function resetPreview(preview) {
    preview.src = '';
}

function setPreview(path, preview) {
    preview.src = path;
}