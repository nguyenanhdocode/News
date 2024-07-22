$(document).ready(() => {
    $('#name').on('change', (e) => {
        const text = e.target.value ?? '';
        $('#slug').val(toSlug(text));
    })
});