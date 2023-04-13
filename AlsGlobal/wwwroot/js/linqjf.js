var isEquals = function (a, b) {
  if (typeof a === "object" && typeof b === "object") {
    var aObject = Object.getOwnPropertyNames(a);
    var bObject = Object.getOwnPropertyNames(b);
    if (aObject.length !== bObject.length)
      return false;
    for (var i = 0; i < aObject.length; i++) {
      var propName = aObject[i];
      if (a[propName] !== b[propName])
        return false;
    }
    return true;
  }
  return false;
};
Array.prototype.groupBy = function (expression) {
  if (typeof expression !== "function")
    throw Error("La expression debe ser función flecha", 10);
  var newObject = [];
  this.forEach(element => {
    var objGroup = { Key: expression(element), Value: [element] };
    if (typeof objGroup.Key === "object") {
      if (newObject.any(x => isEquals(x.Key, objGroup.Key))) {
        newObject.firstOrDefault(x => isEquals(x.Key, objGroup.Key)).Value.push(element);
      }
      else newObject.push(objGroup);
    }
    else {
      if (newObject.any(x => x.Key === objGroup.Key)) {
        newObject.firstOrDefault(x => x.Key === objGroup.Key).Value.push(element);
      }
      else newObject.push(objGroup);
    }

  });
  return newObject;
}
Array.prototype.any = function (expression) {
  var any = false;
  if (typeof expression !== "function")
    throw Error("La expression debe ser función flecha", 10);

  this.forEach(element => {
    any = !any ? expression(element) : any;
  });
  return any;
}
Array.prototype.where = function (expression) {
  var newArray = [];
  if (typeof expression !== "function")
    throw Error("La expression debe ser función flecha", 10);

  this.forEach(element => {
    if (expression(element)) newArray.push(element);
  });
  return newArray;
}
Array.prototype.first = function () {
  var arrayObject = this || [];
  if (arrayObject.length === 0)
    throw Error("No se encontro registros en la lista", 20);
  return arrayObject[0];
}
Array.prototype.firstOrDefault = function (expression) {
  var arrayObject = this || [];
  if (expression != null) arrayObject = arrayObject.where(expression);
  if (arrayObject.length === 0)
    return null;
  return arrayObject[0];
}
Array.prototype.last = function () {
  var arrayObject = this || [];
  if (arrayObject.length === 0)
    throw Error("No se encontro registros en la lista", 20);
  return arrayObject[arrayObject.length - 1];
}
Array.prototype.lastOrDefault = function (expression) {
  var arrayObject = this || [];
  if (expression != null) arrayObject = arrayObject.where(expression);
  if (arrayObject.length === 0)
    return null;
  return arrayObject[arrayObject.length - 1];;
}
Array.prototype.orderBy = function (expression) {
  var arraySort = Object.assign([], this);
  const quickSort = function (array, expression) {
    if (array.length >= 2) {
      var left = [];
      var right = [];
      var index = parseInt(array.length / 2);
      var pivot = array[index];
      for (var i = 0; i < array.length; i++) {
        if (i !== index) {
          if (expression(array[i]) < expression(pivot)) left.push(array[i]);
          else right.push(array[i]);
        }
      }
      if (left.length < right.length) left.push(pivot);
      else right.push(pivot);
      return quickSort(left, expression).concat(quickSort(right, expression));
    }
    else return array;
  };
  return quickSort(arraySort, expression);
}
Array.prototype.orderByDescending = function (expression) {
  var arraySort = Object.assign([], this);
  const quickSort = function (array, expression) {
    if (array.length >= 2) {
      var left = [];
      var right = [];
      var index = parseInt(array.length / 2);
      var pivot = array[index];
      for (var i = 0; i < array.length; i++) {
        if (i !== index) {
          if (expression(array[i]) < expression(pivot)) left.push(array[i]);
          else right.push(array[i]);
        }
      }
      if (left.length < right.length) left.push(pivot);
      else right.push(pivot);
      return quickSort(right, expression).concat(quickSort(left, expression));
    }
    else return array;
  };
  return quickSort(arraySort, expression);
}
Array.prototype.max = function (expression) {
  if (typeof expression !== "function")
    throw Error("La expression debe ser función flecha", 10);
  this.forEach(element => {
    if (typeof expression(element) !== "number")
      throw Error("La expression debe ser función flecha", 10);
  });
  return this.orderBy(expression).last();
}
Array.prototype.min = function (expression) {
  if (typeof expression !== "function")
    throw Error("La expression debe ser función flecha", 10);
  this.forEach(element => {
    if (typeof expression(element) !== "number")
      throw Error("La expression debe ser función flecha", 10);
  });
  return this.orderByDescending(expression).last();
}
Array.prototype.select = function (expression) {
  if (typeof expression !== "function")
    throw Error("La expression debe ser función flecha", 10);
  var newArray = [];
  this.forEach(element => newArray.push(expression(element)));
  return newArray;
}
Array.prototype.all = function (expression) {
  if (typeof expression !== "function")
    throw Error("La expression debe ser función flecha", 10);
  var result = true;
  this.forEach(element => result = result ? expression(element) : false);
  return result;
}
Array.prototype.sum = function (expression) {
  if (typeof expression !== "function")
    throw Error("La expression debe ser función flecha", 10);
  var sum = 0;
  this.forEach(element => {
    if (typeof expression(element) === "number")
      sum += parseFloat(expression(element));
    else throw Error("Solo se aceptan expresiones Number", 10);
  });
  return sum;
}
Array.prototype.count = function (expression) {
  if (expression !== undefined)
    if (typeof expression !== "function")
      throw Error("La expression debe ser función flecha", 10);

  if (expression !== undefined)
    return this.where(expression).length;
  else return this.length;
}
Array.prototype.average = function (expression) {
  if (typeof expression !== "function")
    throw Error("La expression debe ser función flecha", 10);

  return (this.sum(expression) / this.count());
}