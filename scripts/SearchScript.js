window.searchInputHelpText = ' Search entire store...';
function searchTextInput_OnFocus(target) {
    if (target.value == window.searchInputHelpText) {
        target.value = '';
    }
    return true;
};
function searchTextInput_OnBlur(target) {
    if ($.trim(target.value) == '') {
        target.value = window.searchInputHelpText;
    }
    return true;
};
function submitButton_OnClick(event, target, value) {
    event.preventDefault();
    if (!($.trim(value) == '' || value == window.searchInputHelpText)) {
        target.form.submit();
    } else {
        return false;
    }
};