function createCommentItem(form, path) {
    var service = new ItemService({ url: '/sitecore/api/ssc/item' });
    var obj = {
        ItemName: 'comment - ' + form.name.value,
        TemplateID: '{3C6D3C80-6B63-45F2-A9DC-2D13CEF20774}',
        Name: form.name.value,
        Comment: form.comment.value
    };

    service.create(obj)
    .path(path)
    .execute()
    .then(function (item) {
        form.name.value = form.comment.value = '';
        window.alert('Thanks. Your message will appear on the site shortly');
    })
    .fail(function (err) { window.alert(err); });
    event.preventDefault();
    return false;
}