angular.module('umbraco').directive('switch', function() {
  return {
    restrict: 'AE',
    replace: true,
    transclude: true,
    template: function(element, attrs) {
      var html = '';
      html += '<span';
      html +=   ' class="switcher' + (attrs.class ? ' ' + attrs.class : '') + '"';
      html +=   attrs.ngModel ? ' ng-click="' + attrs.ngModel + '=!' + attrs.ngModel + '"' : '';
      html +=   ' ng-class="{ checked:' + attrs.ngModel + ' }"';
      html +=   '>';
      html +=   '<small></small>';
      html +=   '<input type="checkbox"';
      html +=     attrs.id ? ' id="' + attrs.id + '-check"' : '';
      html +=     attrs.name ? ' name="' + attrs.name + '"' : '';
      html +=     attrs.ngModel ? ' ng-model="' + attrs.ngModel + '"' : '';
      html +=     ' style="display:none" />';
      html += '</span>';
      return html;
    }
  }
});
