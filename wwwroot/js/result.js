"use strict";

String.prototype.isNumber = function () {
    return ![null, ""].includes(this) && !isNaN(this);
};

var page = {
    resultEditId: 0,
    resultList: [],
    IsLoadStudentListBlocked: false,
    load: function() {
        this.bindEvents();
        this.loadClassList(function() {
            page.loadSubjectList(function() {
                page.loadResultList();
            });
        });
    },
    bindEvents: function() {
        $("#class").change(function() {
            if (page.IsLoadStudentListBlocked) return;
            page.loadStudentList($(this).val());
        });
        $("#result-list").on("click", ".fa-pencil", function() {
            var id = $(this).closest("tr").attr("data-id");
            var result = page.resultList.find(function(item) {
                return item.id == id;
            });
            if (!result) return;
            page.edit(id);
        });
        $("#result-list").on("click", ".fa-trash", function() {
            var id = $(this).closest("tr").attr("data-id");
            var result = page.resultList.find(function(item) {
                return item.id == id;
            });
            if (!result) return;
            if (confirm("Delete result " + result.name + " ?")) {
                page.delete(id);
            }
        });
    },
    loadClassList: function(callback) {
        var url = "/Result/GetClassList";
        $.get(url, function (result, status) {

            if (result.IsSuccess == false) {
                toastr.error(result.Message);
                return;
            }
            $("#class").html(result.reduce(function(total, item) {
                return total + "<option value='" + item.id + "'>" + item.name + "</option>";
            }, "<option value=''>---Select---</option>"));
            if (callback) { callback(); }
        });
    },
    loadSubjectList: function(callback) {
        var url = "/Result/GetSubjectList";
        $.get(url, function (result, status) {

            if (result.IsSuccess == false) {
                toastr.error(result.Message);
                return;
            }
            $("#subject").html(result.reduce(function(total, item) {
                return total + "<option value='" + item.id + "'>" + item.name + "</option>";
            }, "<option value=''>---Select---</option>"));
            if (callback) { callback(); }
        });
    },
    loadStudentList: function(classId, callback) {
        var url = "/Result/GetStudentList?classId=" + classId;
        $.get(url, function (result, status) {

            if (result.IsSuccess == false) {
                toastr.error(result.Message);
                return;
            }
            $("#student").html(result.reduce(function(total, item) {
                return total + "<option value='" + item.id + "'>" + item.name + "</option>";
            }, "<option value=''>---Select---</option>"));
            if (callback) { callback(); }
        });
    },
    loadResultList: function() {
        var url = "/Result/GetResultList";
        $.get(url, function (result, status) {

            if (result.IsSuccess == false) {
                toastr.error(result.Message);
                return;
            }
            page.resultList = result;
            page.bindResultList();
        });
    },
    bindResultList: function() {
        if (page.resultList.length > 0) {
            $("#result-list tbody").html(page.resultList.reduce(function(total, item) {
                return total +
                "<tr data-id='" + item.id + "'>" +
                    "<td>" + item.className + "</td>" +
                    "<td>" + item.name + "</td>" +
                    "<td>" + item.subjectName + "</td>" +
                    "<td>" + item.mark + "</td>" +
                    "<td><i class='fa fa-pencil' title='Edit'></i></td>" +
                    "<td><i class='fa fa-trash' title='Delete'></i></td>" +
                "</tr>";
            }, ""));
        } else {
            $("#result-list tbody").html("<tr><td colspan='6'>No data found!</td></tr>");
        }
    },
    edit: function(id) {
        var url = "/Result/GetResultDetails?Id=" + id;
        page.resultEditId = 0;
        $.get(url, function (result, status) {

            if (result.IsSuccess == false) {
                toastr.error(result.Message);
                return;
            }
            page.resultEditId = id;
            page.IsLoadStudentListBlocked = true;
            $("#class").val(result.classId);
            $("#subject").val(result.subjectId);
            $("#mark").val(result.mark);
            page.loadStudentList(result.classId, function() {
                $("#student").val(result.studentId);
                page.IsLoadStudentListBlocked = false;
            });
        });
    },
    isValid: function() {
        if ([null, "", "0"].includes($("#student").val())) {
            toastr.error("Student is Required!");
            return false;
        }
        if ([null, "", "0"].includes($("#subject").val())) {
            toastr.error("Subject is Required!");
            return false;
        }
        if ([null, "", "0"].includes($("#mark").val())) {
            toastr.error("Mark is Required!");
            return false;
        }
        if (!($("#student").val()).isNumber()) {
            toastr.error("Student is Invalid!");
            return false;
        }
        if (!($("#subject").val()).isNumber()) {
            toastr.error("Subject is Invalid!");
            return false;
        }
        if (!($("#mark").val()).isNumber()) {
            toastr.error("Mark must be a valid number!");
            return false;
        }
        return true;
    },
    save: function() {
        if (!this.isValid()) return;
        $("#Save").prop("disabled", true);
        var studentId = $("#student").val();
        var subjectId = $("#subject").val();
        var mark = $("#mark").val();
        studentId = studentId.isNumber() ? parseInt(studentId) : 0;
        subjectId = subjectId.isNumber() ? parseInt(subjectId) : 0;
        mark = mark.isNumber() ? parseFloat(mark) : 0;
        $.ajax({
            url: '/Result/Save',
            type: 'Post',
            data: JSON.stringify({
                Id: page.resultEditId,
                StudentId: studentId,
                SubjectId: subjectId,
                Mark: mark
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
                page.loadResultList();
            },
            error: function (data, textStatus, jqXHR) {
                $("#Save").prop("disabled", false);
                toastr.error(data + ": " + textStatus + ": " + jqXHR, 'Error!!!');
            }
        });
    },
    delete: function(id) {
        $.ajax({
            url: '/Result/Delete',
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
                var index = page.resultList.findIndex(function(item) {
                    return item.id == id;
                });
                if (-1 != index) {
                    page.resultList.splice(index, 1);
                    page.bindResultList();
                }
            },
            error: function (data, textStatus, jqXHR) {
                toastr.error(data + ": " + textStatus + ": " + jqXHR, 'Error!!!');
            }
        });
    },
    clear: function() {
        page.resultEditId = 0;
        $("#class").val("");
        $("#student").val("");
        $("#subject").val("");
        $("#mark").val("");
    }
};

$(function() {
    page.load();
});