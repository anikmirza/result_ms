"use strict";

String.prototype.isNumber = function () {
    return ![null, ""].includes(this) && !isNaN(this);
};

var page = {
    studentEditId: 0,
    studentList: [],
    load: function() {
        this.bindEvents();
        this.loadClassList();
        this.loadStudentList();
    },
    bindEvents: function() {
        $("#student-list").on("click", ".fa-pencil", function() {
            var id = $(this).closest("tr").attr("data-id");
            var student = page.studentList.find(function(item) {
                return item.id == id;
            });
            if (!student) return;
            page.edit(id);
        });
        $("#student-list").on("click", ".fa-trash", function() {
            var id = $(this).closest("tr").attr("data-id");
            var student = page.studentList.find(function(item) {
                return item.id == id;
            });
            if (!student) return;
            if (confirm("Delete student " + student.name + " ?")) {
                page.delete(id);
            }
        });
    },
    loadClassList: function() {
        var url = "/Student/GetClassList";
        $.get(url, function (result, status) {

            if (result.IsSuccess == false) {
                toastr.error(result.Message);
                return;
            }
            $("#class").html(result.reduce(function(total, item) {
                return total + "<option value='" + item.id + "'>" + item.name + "</option>";
            }, "<option value=''>---Select---</option>"));
        });
    },
    loadStudentList: function() {
        var url = "/Student/GetStudentList";
        $.get(url, function (result, status) {

            if (result.IsSuccess == false) {
                toastr.error(result.Message);
                return;
            }
            page.studentList = result;
            page.bindStudentList();
        });
    },
    bindStudentList: function() {
        if (page.studentList.length > 0) {
            $("#student-list tbody").html(page.studentList.reduce(function(total, item) {
                return total +
                "<tr data-id='" + item.id + "'>" +
                    "<td>" + item.name + "</td>" +
                    "<td>" + item.roll + "</td>" +
                    "<td>" + item.className + "</td>" +
                    "<td><i class='fa fa-pencil' title='Edit'></i></td>" +
                    "<td><i class='fa fa-trash' title='Delete'></i></td>" +
                "</tr>";
            }, ""));
        } else {
            $("#student-list tbody").html("<tr><td colspan='5'>No data found!</td></tr>");
        }
    },
    edit: function(id) {
        var url = "/Student/GetStudentDetails?Id=" + id;
        page.studentEditId = 0;
        $.get(url, function (result, status) {

            if (result.IsSuccess == false) {
                toastr.error(result.Message);
                return;
            }
            page.studentEditId = id;
            $("#class").val(result.classId);
            $("#name").val(result.name);
            $("#roll").val(result.roll);
            $("#phone").val(result.phone);
            $("#email").val(result.email);
        });
    },
    isValid: function() {
        if ([null, ""].includes($("#name").val())) {
            toastr.error("Name is Required!");
            return false;
        }
        if ([null, "", "0"].includes($("#class").val())) {
            toastr.error("Class is Required!");
            return false;
        }
        if ([null, "", "0"].includes($("#roll").val())) {
            toastr.error("Roll is Required!");
            return false;
        }
        if (!($("#class").val()).isNumber()) {
            toastr.error("Class is Invalid!");
            return false;
        }
        if (!($("#roll").val()).isNumber()) {
            toastr.error("Roll must be a valid number!");
            return false;
        }
        return true;
    },
    save: function() {
        if (!this.isValid()) return;
        $("#Save").prop("disabled", true);
        var classId = $("#class").val();
        var roll = $("#roll").val();
        classId = classId.isNumber() ? parseInt(classId) : 0;
        roll = roll.isNumber() ? parseInt(roll) : 0;
        $.ajax({
            url: '/Student/Save',
            type: 'Post',
            data: JSON.stringify({
                Id: page.studentEditId,
                ClassId: classId,
                Name: $("#name").val(),
                Roll: roll,
                Phone: $("#phone").val(),
                Email: $("#email").val()
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
                page.loadStudentList();
            },
            error: function (data, textStatus, jqXHR) {
                $("#Save").prop("disabled", false);
                toastr.error(data + ": " + textStatus + ": " + jqXHR, 'Error!!!');
            }
        });
    },
    delete: function(id) {
        $.ajax({
            url: '/Student/Delete',
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
                var index = page.studentList.findIndex(function(item) {
                    return item.id == id;
                });
                if (-1 != index) {
                    page.studentList.splice(index, 1);
                    page.bindStudentList();
                }
            },
            error: function (data, textStatus, jqXHR) {
                toastr.error(data + ": " + textStatus + ": " + jqXHR, 'Error!!!');
            }
        });
    },
    clear: function() {
        page.studentEditId = 0;
        $("#class").val("");
        $("#name").val("");
        $("#roll").val("");
        $("#phone").val("");
        $("#email").val("");
    }
};

$(function() {
    page.load();
});