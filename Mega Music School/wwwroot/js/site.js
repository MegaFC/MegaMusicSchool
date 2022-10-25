//ADMIN POST VIDEOS THAT IS ACCEPTED
function AcceptVideoPost(Id) {
    debugger;
    $.ajax({
        type: 'POST',
        dataType: 'Json',
        url: '/Admin/AcceptVideos',
        data:
        {
            videoId: Id
        },
        success: function (result) {
            debugger;
            if (!result.isError) {
                var FUrl = "/Admin/VideoStatus";
                AcceptAlert(result.msg, FUrl)
            }
            else {
                errorAlert(result.msg)
            }
        },
        Error: function (ex) {
            errorAlert(ex);
        }
    });
}



// ADMIN DELETE VIDEO THAT IS NOT ACCEPTED
function RejectVideoPost(Id) {
    debugger;
    $.ajax({
        type: 'POST',
        dataType: 'Json',
        url: '/Admin/RejectVideos',
        data:
        {
            videoId: Id
        },
        success: function (result) {
            debugger;
            if (!result.isError) {
                var ZUrl = "/Admin/VideoStatus";
                rejectAlert(result.msg, ZUrl)
            }
            else {
                errorAlert(result.msg)
            }
        },
        Error: function (ex) {
            errorAlert(ex);
        }
    });
}


// ADMIN DELETE VIDEO THAT IS NOT ACCEPTED
function EludeCourse(Id) {
    debugger;
    $.ajax({
        type: 'POST',
        dataType: 'Json',
        url: '/Admin/DeleteCourse',
        data:
        {
            courseId: Id
        },
        success: function (result) {
            debugger;
            if (!result.isError) {
                var ElUrl = "/Admin/AllAddedCourses";
                EludeAlert(result.msg, ElUrl)
            }
            else {
                errorAlert(result.msg)
            }
        },
        Error: function (ex) {
            errorAlert(ex);
        }
    });
}


// STUDENT DELETE VIDEO
function DeleteItem(Id) {
    debugger;
    $.ajax({
        type: 'POST',
        dataType: 'Json',
        url: '/Student/DeleteUpload',
        data:
        {
            studentId: Id
        },
        success: function (result) {
            debugger;
            if (!result.isError) {
                var nextUrl = "/Student/AllUploadedItems";
                successAlert(result.msg, nextUrl)
            }
            else {
                errorAlert(result.msg)
            }
        },
        Error: function (ex) {
            errorAlert(ex);
        }
    });
}

// STUDENT CHOOSE COURSES
function Achieve(Id) {
    debugger;
    $.ajax({
        type: 'POST',
        dataType: 'Json',
        url: '/Student/SelectCourse',
        data:
        {
            CourseDirectingId: Id
        },
        success: function (result) {
            debugger;
            if (!result.isError) {
                successAlert(result.msg)
                window.location.reload();
            }
            else {
                errorAlert(result.msg)
            }
        },
        Error: function (ex) {
            errorAlert(ex);
        }
    });
}



// ADMIN DELETE STUDENT
function Expell(Id) {
    debugger;
    $.ajax({
        type: 'POST',
        dataType: 'Json',
        url: '/Admin/DeleteStudent',
        data:
        {
            studentField: Id
        },
        success: function (result) {
            debugger;
            if (!result.isError) {
                var AEUrl = "/Admin/AdminEffect";
                successAlertWithRedirect(result.msg, AEUrl)
            }
            else {
                errorAlert(result.msg)
            }
        },
        Error: function (ex) {
            errorAlert(ex);
        }
    });
}