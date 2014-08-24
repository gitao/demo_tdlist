$(function () {
    var tmpl_msg = "<li id=\"todo-{0}\" class=\"todo\"><div class=\"text\">{1}</div><div class=\"actions\"><a href=\"#\" class=\"edit\" title=\"Edit\">Edit</a><a href=\"#\" class=\"complete\" title=\"Complete\">Complete</a><a href=\"#\" class=\"restore\" style=\"display:none\" title=\"Restore\">Restore</a><a href=\"#\" class=\"delete\" title=\"Delete\">Delete</a></div></li>";
    $.ajaxSetup({ traditional: true });

    $(".todoList").sortable({
        axis: 'y',
        containment: 'window',
        update: function () {            
            var arr = $(".todoList").sortable('toArray');            
            arr = $.map(arr, function (val, key) {
                return val.replace('todo-', '');
            });            
            $.post('/Home/ReArrange', { 'ids': arr }, function (data) {
                if (data.Id == 0) {
                    alert(data.Message);
                }                
            });
        },
        stop: function (e, ui) {
            ui.item.css({ 'top': '0', 'left': '0' });
        }
    });

    var currentTODO;

    $("#dialog-confirm").dialog({
        resizable: false,
        height: 130,
        modal: true,
        autoOpen: false,
        buttons: {
            'Delete': function () {
                $.post("/Home/Delete", { "id": currentTODO.data('id') }, function (data) {
                    if (data.Id == 0) {
                        alert(data.Message);
                    }
                    else {
                        currentTODO.fadeOut('fast');
                    }
                });
                $(this).dialog('close');
            },
            Cancel: function () {
                $(this).dialog('close');
            }
        }
    });

    $("#dialog-add").dialog({
        resizable: false,
        height: 130,
        modal: true,
        autoOpen: false,
        buttons: {
            'Add': function () {
                var datamsg = $("#txt_additem").val().replace(/(<([^>]+)>)/ig, '');
                if (datamsg != "") {
                    var msg = tmpl_msg.replace("{1}", datamsg);
                    $.post("/Home/Add", { 'item': datamsg }, function (data) {
                        if (data.Id == 0) {
                            alert(data.Message);
                        }
                        else {
                            msg = msg.replace("{0}", data.Id)
                            $(msg).hide().appendTo('.todoList').fadeIn();
                        }
                    });
                }
                else {
                    $.alert("Please enter a valid to-do list item, the content of to-do list item cannot be empty.", "Warning");
                }
                $("#txt_additem").val("");
                $(this).dialog('close');                
            },
            Cancel: function () {
                $(this).dialog('close');
            }
        }
    });

    /*
    $('.todo').live('dblclick', function () {
        $(this).find('a.edit').click();
    });
    */

    $('.todo a').live('click', function (e) {
        currentTODO = $(this).closest('.todo');
        currentTODO.data('id', currentTODO.attr('id').replace('todo-', ''));

        e.preventDefault();
    });

    $('.todo a.delete').live('click', function () {
        $("#dialog-confirm").dialog('open');
    });

    $('.todo a.complete').live('click', function () {
        $.post("/Home/Complete", { "id": currentTODO.data('id') }, function (data) {
            if (data.Id == 0) {
                alert(data.Message);
            }
            else {
                var container = currentTODO.find('.text');
                container.css("text-decoration", "line-through");
                container = currentTODO.find('.actions .edit');
                container.css("display", "none");
                container = currentTODO.find('.actions .complete');
                container.css("display", "none");
                container = currentTODO.find('.actions .restore');
                container.css("display", "block");
            }
        });
    });

    $('.todo a.restore').live('click', function () {
        $.post("/Home/Restore", { "id": currentTODO.data('id') }, function (data) {
            if (data.Id == 0) {
                alert(data.Message);
            }
            else {
                var container = currentTODO.find('.text');
                container.css("text-decoration", "none");
                container = currentTODO.find('.actions .edit');
                container.css("display", "block");
                container = currentTODO.find('.actions .complete');
                container.css("display", "block");
                container = currentTODO.find('.actions .restore');
                container.css("display", "none");
            }
        });
    });

    $('.todo a.edit').live('click', function () {

        var container = currentTODO.find('.text');

        if (!currentTODO.data('origText')) {
            currentTODO.data('origText', container.text());
        }
        else {
            return false;
        }

        $('<input type="text">').val(container.text()).appendTo(container.empty());

        container.append(
			'<br/><div class="editTodo">' +
				'<a class="saveChanges" href="#">Save</a> or <a class="discardChanges" href="#">Cancel</a>' +
			'</div>'
		);
    });

    $('.todo a.discardChanges').live('click', function () {
        currentTODO.find('.text')
					.text(currentTODO.data('origText'))
					.end()
					.removeData('origText');
    });

    $('.todo a.saveChanges').live('click', function () {
        var text = currentTODO.find("input[type=text]").val().replace(/(<([^>]+)>)/ig, '');

        if (text != "") {
            $.post("/Home/Update", { 'id': currentTODO.data('id'), 'item': text }, function (data) {
                if (data.Id == 0) {
                    alert(data.Message);
                }
                else {
                    currentTODO.removeData('origText')
                        .find(".text")
                        .text(text);
                }
            });
        }
        else {
            $.alert("Please enter a valid to-do list item, the content of to-do list item cannot be empty.", "Warning");
        }        
    });

    $('#addButton').click(function (e) {

        $("#dialog-add").dialog('open');

        e.preventDefault();
    });

    $.extend({
        alert: function (message, title) {
            $("<div></div>").dialog({
                buttons: { "Ok": function () { $(this).dialog("close"); } },
                close: function (event, ui) { $(this).remove(); },
                resizable: false,
                title: title,
                modal: true
            }).text(message);
        }
    });
}); // Closing $(document).ready()