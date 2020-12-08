declare global {
  interface Array<T> {
    last(): T;
  }
}

Array.prototype.last = function<T>(): T {
  return this[this.length - 1];
}

export default {};