

var todo = {
  data: [], // holder for todo list array
  itemStatus: { NOTDONE:0, COMPLETED:1, CANCELLED:2 },
  deleteStatus: { ALL:0, COMPLETED:1 },
  workItemIndexes: { ID:0, DESCRIPTION:1, STATUS:2 },
  mongoStorage: [],
  load: function () {
  // todo.load() : attempt to load todo list from local storage
    $.ajax({
      url:"/Home/GetToDoItems",
      method: 'GET',
      success: function(data){   
        var todoListFormatted = data.map(d => '["' + d.id + '","' + d.workItemDescription + '",' + d.state + ']');
     
        mongoStorage = "[" + todoListFormatted + "]";       
        todo.data = JSON.parse(mongoStorage);
       
        todo.list();
      }
    });
  },
  deleteAll: function(){
    $.ajax({
      url:"/Home/DeleteAllToDoItems",
      method: 'POST',
      success: function(data){   
        todo.load();
      }
    });  
    mongoStorage = JSON.stringify(todo.data);
    todo.list();
  },
  deleteCompleted: function(){
    $.ajax({
      url:"/Home/DeleteCompletedToDoItems",
      method: 'POST',
      success: function(data){   
        todo.load();
      }
    });  
    mongoStorage = JSON.stringify(todo.data);
    todo.list();
  },
  save: function (itemToSave) {
  // todo.save() : save the current data to mongo storage  
    $.ajax({
      url:"/Home/SaveToDoItems",
      method: 'POST',
      data: { 'item': {id: itemToSave[todo.workItemIndexes.ID], workItemDescription: itemToSave[todo.workItemIndexes.DESCRIPTION], state: itemToSave[todo.workItemIndexes.STATUS]}},
      success: function(data){   
        todo.load();
      }
    });  
    mongoStorage = JSON.stringify(todo.data);
    todo.list();
  },

  list: function () {
  // todo.list() : update todo list HTML

    // Clear the old list
    var container = document.getElementById("todo-list");
    container.innerHTML = "";

    // Rebuild list
    if (todo.data.length > 0) {
      var row = "", el = "";
      for (var key in todo.data) {
        // Row container
        row = document.createElement("div");
        row.classList.add("clearfix");
        row.dataset.id = key;

        // Item text
        el = document.createElement("div");
        el.classList.add("item");
        if (todo.data[key][todo.workItemIndexes.STATUS] == todo.itemStatus.COMPLETED) {
          el.classList.add("done");
        }
        if (todo.data[key][todo.workItemIndexes.STATUS] == todo.itemStatus.CANCELLED) {
          el.classList.add("cx");
        }
        el.innerHTML = todo.data[key][todo.workItemIndexes.DESCRIPTION];
        row.appendChild(el);

        // Add cancel button
        el = document.createElement("input");
        el.setAttribute("type", "button");
        el.value = "\u2716";
        el.classList.add("bdel");
        el.addEventListener("click", function () {
          todo.status(this, todo.itemStatus.CANCELLED);
        });
        row.appendChild(el);

        // Add done button
        el = document.createElement("input");
        el.setAttribute("type", "button");
        el.value = "\u2714";
        el.classList.add("bdone");
        el.addEventListener("click", function () {
          todo.status(this, todo.itemStatus.COMPLETED);
        });
        row.appendChild(el);

        // Add row to list
        container.appendChild(row);
      }
    }
  },

  add: function () {
  // todo.add() : add a new item

    todo.data.push([
      "",document.getElementById("todo-add").value, todo.itemStatus.NOTDONE
    ]);
    todo.save(["",document.getElementById("todo-add").value, todo.itemStatus.NOTDONE]);
    document.getElementById("todo-add").value = "";
  },

  status: function (el, stat) {
  // todo.status() : update item status

    var parent = el.parentElement;
    todo.data[parent.dataset.id][todo.workItemIndexes.STATUS] = stat;
    todo.save(todo.data[parent.dataset.id]);
  },

  del: function (type) {
  // todo.del() : delete items

  if (confirm("Delete tasks?")) {
      // Delete all
      if (type == todo.deleteStatus.ALL) {
        todo.deleteAll();
      }
      // Filter, keep only not completed
      else {
        todo.deleteCompleted();
      }
    }
  }
};

// Page init
window.addEventListener("load", function () {
  document.getElementById("todo-da").addEventListener("click", function () {
    todo.del(todo.deleteStatus.ALL);
  });
  document.getElementById("todo-dc").addEventListener("click", function () {
    todo.del(todo.deleteStatus.COMPLETED);
  });
  document.getElementById("todo-form").addEventListener("submit", function (evt) {
    evt.preventDefault();
    todo.add();
  });
  todo.load();
});