"use strict";

String.prototype.isNumber = function () {
    return ![null, ""].includes(this) && !isNaN(this);
};

var page = {
    subjectEditId: 0,
    subjectList: [],
    load: function() {
        this.bindEvents();
        this.loadSubjectList();
    },
    bindEvents: function() {
        $("#subject-list").on("click", ".fa-pencil", function() {
            var id = $(this).closest("tr").attr("data-id");
            var subject = page.subjectList.find(function(item) {
                return item.id == id;
            });
            if (!subject) return;
            page.edit(id);
        });
        $("#subject-list").on("click", ".fa-trash", function() {
            var id = $(this).closest("tr").attr("data-id");
            var subject = page.subjectList.find(function(item) {
                return item.id == id;
            });
            if (!subject) return;
            if (confirm("Delete subject " + subject.name + " ?")) {
                page.delete(id);
            }
        });
    },
    loadSubjectList: function() {
        var url = "/Subject/GetSubjectList";
        $.get(url, function (result, status) {

            if (result.IsSuccess == false) {
                toastr.error(result.Message);
                return;
            }
            page.subjectList = result;
            page.bindSubjectList();
        });
    },
    bindSubjectList: function() {
        if (page.subjectList.length > 0) {
            $("#subject-list tbody").html(page.subjectList.reduce(function(total, item) {
                return total +
                "<tr data-id='" + item.id + "'>" +
                    "<td>" + item.name + "</td>" +
                    "<td><i class='fa fa-pencil' title='Edit'></i></td>" +
                    "<td><i class='fa fa-trash' title='Delete'></i></td>" +
                "</tr>";
            }, ""));
        } else {
            $("#subject-list tbody").html("<tr><td colspan='3'>No data found!</td></tr>");
        }
    },
    edit: function(id) {
        page.subjectEditId = id;
        var subjectFound = page.subjectList.find(function(item) {
            return item.id == id;
        })
        if (subjectFound) {
            $("#name").val(subjectFound.name);
        }
    },
    isValid: function() {
        if ([null, ""].includes($("#name").val())) {
            toastr.error("Name is Required!");
            return false;
        }
        return true;
    },
    save: function() {
        if (!this.isValid()) return;
        $("#Save").prop("disabled", true);
        $.ajax({
            url: '/Subject/Save',
            type: 'Post',
            data: JSON.stringify({
                Id: page.subjectEditId,
                Name: $("#name").val()
            }),
            dataType: 'json',
            async: true,
            contentType: 'application/json',
            success: function (result) {
                $("#Save").prop("disabled", false);

                if (result.IsSuccess == false) {
                    toastr.error(result.Message);
                    return;
                }
                toastr.success("Data save successfully!");
                page.clear();
                page.loadSubjectList();
            },
            error: function (data, textStatus, jqXHR) {
                $("#Save").prop("disabled", false);
                toastr.error(data + ": " + textStatus + ": " + jqXHR, 'Error!!!');
            }
        });
    },
    delete: function(id) {
        $.ajax({
            url: '/Subject/Delete',
            type: 'Post',
            data: JSON.stringify(id),
            dataType: 'json',
            async: true,
            contentType: 'application/json',
            success: function (result) {

                if (result.IsSuccess == false) {
                    toastr.error(result.Message);
                    return;
                }
                toastr.success("Data removed successfully!");
                var index = page.subjectList.findIndex(function(item) {
                    return item.id == id;
                });
                if (-1 != index) {
                    page.subjectList.splice(index, 1);
                    page.bindSubjectList();
                }
            },
            error: function (data, textStatus, jqXHR) {
                toastr.error(data + ": " + textStatus + ": " + jqXHR, 'Error!!!');
            }
        });
    },
    clear: function() {
        page.subjectEditId = 0;
        $("#name").val("");
    }
};

$(function() {
    page.load();
});