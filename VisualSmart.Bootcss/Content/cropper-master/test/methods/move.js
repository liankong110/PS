$(function () {

  'use strict';

  var $image = $(window.createCropperImage());

  function random(offset) {
    var n = Math.random() * offset;

    return parseInt(n, 10) % 2 === 0 ? -n : n;
  }

  $image.cropper({
    strict: false,

    built: function () {
      var cropper = $(this).data('cropper');
      var canvas = cropper.canvas;
      var offsets = (function () {
            var data = [],
                max = 10,
                i = 10;

            while (i--) {
              data.push({
                x: random(max),
                y: random(max)
              });
            }

            return data;
          })();

      QUnit.test('methods.move', function (assert) {
        $.each(offsets, function (i, offset) {
          var left = canvas.left + offset.x;
          var top = canvas.top + offset.y;

          $image.cropper('move', offset.x, offset.y);

          assert.ok(canvas.left === left);
          assert.ok(canvas.top === top);
        });
      });

    }
  });

});
