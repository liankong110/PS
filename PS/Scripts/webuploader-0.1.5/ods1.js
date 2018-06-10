// 文件上传
jQuery(function () {
    var $ = jQuery,
        $list = $('#thelist'),
        $btn = $('#ctlBtn'),
        state = 'pending',
        uploader;
    var url = $("#server").val();

    uploader = WebUploader.create({
        fileNumLimit: 1,
        auto: true,
        // 不压缩image
        resize: false,
        fileSizeLimit: 10 * 1024 * 1024,
        fileSingleSizeLimit: 10 * 1024 * 1024,
        //duplicate: true,
        // swf文件路径
        swf: 'Uploader.swf',

        // 文件接收服务端。
        server: url,

        // 选择文件的按钮。可选。
        // 内部根据当前运行是创建，可能是input元素，也可能是flash.
        pick: '#picker',
        // 只允许选择文件，可选。
        accept: {
            title: '文件',
            extensions: 'xls,xlsx',
            mimeTypes: 'application/vnd.ms-excel,application/vnd.openxmlformats-officedocument.spreadsheetml.sheet'
        }
    });

    // 当有文件添加进来的时候
    uploader.on('fileQueued', function (file) {
        //var fileName = "_" + $("#QAD").val() + ".pdf";
        //if (file.name.indexOf(fileName) <= 0) {
        //    alert("ODS的文件名，必须满足“产线_QAD号”的格式");
        //    return false;
        //}
        $list.append('<div id="' + file.id + '" class="item">' +
            '<h4 class="info">' + file.name + '</h4><a class=\"ion-close-round\" href="#" >删除</a>' +
            '<span class="state" style="margin-left:10px;">等待上传...</span>' +
        '</div>');
    });

    // 文件上传过程中创建进度条实时显示。
    uploader.on('uploadProgress', function (file, percentage) {
        var $li = $('#' + file.id),
            $percent = $li.find('.progress .progress-bar');

        // 避免重复创建
        if (!$percent.length) {
            $percent = $('<div class="progress progress-striped active">' +
              '<div class="progress-bar" role="progressbar" style="width: 0%">' +
              '</div>' +
            '</div>').appendTo($li).find('.progress-bar');
        }

        $li.find('span.state').text('上传中');

        $percent.css('width', percentage * 100 + '%');
    });

    uploader.on('uploadSuccess', function (file, data) {
    
        $("#Title").val(file.name);
        if (data._raw != "success") {
            $('#' + file.id).find('span.state').text(data._raw);
            $('#' + file.id).find('span.state').css("color","red");
        } else {
            $('#' + file.id).find('span.state').text('已上传，并解析成功！');
        }
      
      
    });

    uploader.on('error', function (handler, file) {
        if (handler == "Q_EXCEED_NUM_LIMIT") {
            layer.msg("最多传1个附件");
        } else if (handler == "F_DUPLICATE") {
            layer.msg("文件已存在队列中");
        }
        alert(handler);
    });

    // 所有文件上传成功后调用        
    uploader.on('uploadFinished', function () {
        //清空队列
        //uploader.reset();      
    });

    uploader.on('uploadError', function (handler, file) {        
        $('#' + file.id).find('p.state').text('上传出错');       
    });


    $list.on("click", ".ion-close-round", function () {
        var fileItem = $(this).parent();
        var id = $(fileItem).attr("id");
        uploader.removeFile(id, true);
        $(fileItem).fadeOut(function () {
            $(fileItem).remove();
        });
        
    });

    uploader.on('uploadComplete', function (file) {
        $('#' + file.id).find('.progress').fadeOut();
    });

    uploader.on('all', function (type) {
        if (type === 'startUpload') {
            state = 'uploading';
        } else if (type === 'stopUpload') {
            state = 'paused';
        } else if (type === 'uploadFinished') {
            state = 'done';
        }

        if (state === 'uploading') {
            $btn.text('暂停上传');
        } else {
            $btn.text('开始上传');
        }
    });

    $btn.on('click', function () {
        if (state === 'uploading') {
            uploader.stop();
        } else {
            uploader.upload();
        }
    });
});


// 图片上传demo
jQuery(function () {
    var $ = jQuery,
        $list = $('#fileList'),
        // 优化retina, 在retina下这个值是2
        ratio = window.devicePixelRatio || 1,

        // 缩略图大小
        thumbnailWidth = 100 * ratio,
        thumbnailHeight = 100 * ratio,

        // Web Uploader实例
        uploader;

    // 初始化Web Uploader
    uploader = WebUploader.create({

        // 自动上传。
        auto: true,

        // swf文件路径
        swf: 'Uploader.swf',

        // 文件接收服务端。
        server: '/WFUploadFile.aspx',

        // 选择文件的按钮。可选。
        // 内部根据当前运行是创建，可能是input元素，也可能是flash.
        pick: '#filePicker',

        // 只允许选择文件，可选。
        accept: {
            title: 'Images',
            extensions: 'gif,jpg,jpeg,bmp,png',
            mimeTypes: 'image/*'
        }
    });

    // 当有文件添加进来的时候
    uploader.on('fileQueued', function (file) {
        var $li = $(
                '<div id="' + file.id + '" class="file-item thumbnail">' +
                    '<img>' +
                    '<div class="info">' + file.name + '</div>' +
                '</div>'
                ),
            $img = $li.find('img');

        $list.append($li);

        // 创建缩略图
        uploader.makeThumb(file, function (error, src) {
            if (error) {
                $img.replaceWith('<span>不能预览</span>');
                return;
            }

            $img.attr('src', src);
        }, thumbnailWidth, thumbnailHeight);
    });

    // 文件上传过程中创建进度条实时显示。
    uploader.on('uploadProgress', function (file, percentage) {
        var $li = $('#' + file.id),
            $percent = $li.find('.progress span');

        // 避免重复创建
        if (!$percent.length) {
            $percent = $('<p class="progress"><span></span></p>')
                    .appendTo($li)
                    .find('span');
        }

        $percent.css('width', percentage * 100 + '%');
    });

    // 文件上传成功，给item添加成功class, 用样式标记上传成功。
    uploader.on('uploadSuccess', function (file) {
        $('#' + file.id).addClass('upload-state-done');
    });

    // 文件上传失败，现实上传出错。
    uploader.on('uploadError', function (file) {
        var $li = $('#' + file.id),
            $error = $li.find('div.error');

        // 避免重复创建
        if (!$error.length) {
            $error = $('<div class="error"></div>').appendTo($li);
        }

        $error.text('上传失败');
    });

    // 完成上传完了，成功或者失败，先删除进度条。
    uploader.on('uploadComplete', function (file) {
        $('#' + file.id).find('.progress').remove();
    });
});