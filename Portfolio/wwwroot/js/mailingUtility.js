document.getElementById("contact-me-form")
    .addEventListener("submit", async function (e) {
        e.preventDefault();

        tinymce.triggerSave();

        const form = e.target;
        const formData = new FormData(form);

        const response = await fetch("/User/Contact/Submit", {
            method: "POST",
            body: formData
        });

        const result = await response.status;

        if (result != 200) {

            return confirm('B³¹d wysy³ki..');
        }

        document.getElementById("contact-me-form").style = 'Display:none';
        document.getElementById("contact-me-result").style = 'Display:inline';
    });