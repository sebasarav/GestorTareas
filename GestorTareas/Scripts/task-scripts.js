/**
 * task-scripts.js
 * GestorTareas – shared client-side utilities
 *
 * Loaded on every page via _Layout.cshtml.
 * Page-specific logic lives in @section scripts blocks within each view.
 */

$(document).ready(function () {

    /**
     * Auto-dismiss Bootstrap dismissible alerts after 6 seconds.
     * Applies to TempData messages rendered by the layout (success, error, warning).
     */
    setTimeout(function () {
        $('.alert-dismissible').each(function () {
            $(this).fadeOut(500, function () {
                $(this).remove();
            });
        });
    }, 6000);

    /**
     * Enhance all Bootstrap tooltips on the page (if any element uses data-toggle="tooltip").
     */
    $('[data-toggle="tooltip"]').tooltip();

});

/**
 * Escapes HTML special characters to prevent XSS when inserting
 * user-generated content into dynamically built HTML strings.
 *
 * @param  {string} text - Raw text to escape
 * @returns {string} HTML-safe string
 */
function escHtml(text) {
    if (text == null) return '';
    return String(text)
        .replace(/&/g,  '&amp;')
        .replace(/</g,  '&lt;')
        .replace(/>/g,  '&gt;')
        .replace(/"/g,  '&quot;')
        .replace(/'/g,  '&#039;');
}
