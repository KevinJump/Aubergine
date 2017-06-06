$(function () {
    tinymce.init({
        selector: '#Body',
        menubar: false,
        branding: false,
        statusbar: false,
        plugins: ['link', 'lists'],
        toolbar: 'undo redo | bold italic | link | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent'
    });
});
