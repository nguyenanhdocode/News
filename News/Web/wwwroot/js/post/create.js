
$(document).ready(() => {

    $('#title').on('change', (e) => {
        const text = e.target.value ?? '';
        $('#slug').val(toSlug(text));
    })

    let editor = null;
    ClassicEditor
        .create(document.querySelector('#editor'), {
            toolbar: [
                'undo', 'redo',
                '|',
                'heading',
                '|',
                'fontfamily', 'fontsize', 'fontColor', 'fontBackgroundColor',
                '|',
                'bold', 'italic', 'strikethrough', 'subscript', 'superscript',
                '|',
                'link', 'insertImage', 'blockQuote',
                '|',
                'bulletedList', 'numberedList', 'todoList', 'outdent', 'indent',
                '|',
                'alignment'
            ]
        })
        .then(e => {
            editor = e;
        })
        .catch(error => {
            console.error(error);
        });

    $(document).ready(() => {
        editor.setData($('#content').val());
    });

    $('#btnSave').on('click', (e) => {
        $('#content').val(editor.getData());
    });
})