"use strict";

String.prototype.isNumber = function () {
    return ![null, ""].includes(this) && !isNaN(this);
};

var page = {
    classEditId: 0,
    classList: [],
    load: function() {
        this.bindEvents();
        this.loadClassList();
    },
    bindEvents: function() {
        $("#class-list").on("click", ".fa-pencil", function() {
            var id = $(this).closest("tr").attr("data-id");
            var classObj = page.classList.find(function(item) {
                return item.id == id;
            });
            if (!classObj) return;
            page.edit(id);
        });
        $("#class-list").on("click", ".fa-trash", function() {
            var id = $(this).closest("tr").attr("data-id");
            var classObj = page.classList.find(function(item) {
                return item.id == id;
            });
            if (!classObj) return;
            if (confirm("Delete class " + classObj.name + " ?")) {
                page.delete(id);
            }
        });
    },
    loadClassList: function() {
        var url = "/Class/GetClassList";
        $.get(url, function (result, status) {

            if (result.IsSuccess == false) {
                toastr.error(result.Message);
                return;
            }
            page.classList = result;
            page.bindClassList();
        });
    },
    bindClassList: function() {
        if (page.classList.length > 0) {
            $("#class-list tbody").html(page.classList.reduce(function(total, item) {
                return total +
                "<tr data-id='" + item.id + "'>" +
                    "<td>" + item.name + "</td>" +
                    "<td>" + item.section + "</td>" +
                    "<td><i class='fa fa-pencil' title='Edit'></i></td>" +
                    "<td><i class='fa fa-trash' title='Delete'></i></td>" +
                "</tr>";
            }, ""));
        } else {
            $("#class-list tbody").html("<tr><td colspan='4'>No data found!</td></tr>");
        }
    },
    edit: function(id) {
        page.classEditId = id;
        var classFound = page.classList.find(function(item) {
            return item.id == id;
        });
        if (classFound) {
            $("#name").val(classFound.name);
            $("#section").val(classFound.section);
        }
    },
    isValid: function() {
        if ([null, ""].includes($("#name").val())) {
            toastr.error("Name is Required!");
            return false;
        }
        if ([null, ""].includes($("#section").val())) {
            toastr.error("Section is Required!");
            return false;
        }
        return true;
    },
    save: function() {
        if (!this.isValid()) return;
        $("#Save").prop("disabled", true);
        $.ajax({
            url: '/Class/Save',
            type: 'Post',
            data: JSON.stringify({
                Id: page.classEditId,
                Name: $("#name").val(),
                Section: $("#section").val()
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
                page.loadClassList();
            },
            error: function (data, textStatus, jqXHR) {
                $("#Save").prop("disabled", false);
                toastr.error(data + ": " + textStatus + ": " + jqXHR, 'Error!!!');
            }
        });
    },
    delete: function(id) {
        $.ajax({
            url: '/Class/Delete',
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
                var index = page.classList.findIndex(function(item) {
                    return item.id == id;
                });
                if (-1 != index) {
                    page.classList.splice(index, 1);
                    page.bindClassList();
                }
            },
            error: function (data, textStatus, jqXHR) {
                toastr.error(data + ": " + textStatus + ": " + jqXHR, 'Error!!!');
            }
        });
    },
    clear: function() {
        page.classEditId = 0;
        $("#name").val("");
        $("#section").val("");
    }
};

$(function() {
    page.load();
});