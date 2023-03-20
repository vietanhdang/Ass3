// function format date dd/MM/yyyy hh:mm
function formatDate(date) {
  var d = new Date(date),
    month = "" + (d.getMonth() + 1),
    day = "" + d.getDate(),
    year = d.getFullYear(),
    hour = d.getHours(),
    minute = d.getMinutes();
  if (month.length < 2) month = "0" + month;
  if (day.length < 2) day = "0" + day;
  if (hour.length < 2) hour = "0" + hour;
  if (minute.length < 2) minute = "0" + minute;
  return [day, month, year].join("/") + " " + [hour, minute].join(":");
}

function createNewPost(post) {
  return `<div class="card col-md-4 m-2 style="width: 18rem;" id="post-${post.postID}">
    <div class="card-body">
        <h5 class="card-title"> ${post.title} </h5>
        <a href="javascript:void(0)" class="card-subtitle mb-2 text-muted">@item.Author.Email</a>
        <p class="card-text my-2">${post.content}</p>
        <p class="card-link">${formatDate(post.updatedDate)}</p>
    </div>
</div>`;
}
var connection = new signalR.HubConnectionBuilder().withUrl("/signalRServer").build();
connection
  .start()
  .then(function () {
    console.log("SignalR Started...");
  })
  .catch(function (err) {
    return console.error(err);
  });

connection.on("CreatePost", function (post, email) {
  $("#post-list").prepend(createNewPost(post));
  $.toast({
    heading: "Create post",
    text: `Post ${post.title} has been updated by ${email}`,
    icon: "info",
    loader: true, // Change it to false to disable loader
    loaderBg: "#9EC600", // To change the background
  });
});

connection.on("UpdatePost", function (post, email) {
  console.log(post);
  let postId = post.postID;
  if ($(`#post-${postId}`).length) {
    if (!post.publishStatus) {
      $(`#post-${postId}`).remove();
      return;
    }
    let content = $(`#post-${postId} .card-text`);
    content.text(post.content);
    let date = $(`#post-${postId} .card-link`);
    date.text(formatDate(post.updatedDate));
    let title = $(`#post-${postId} .card-title`);
    title.text(post.title);
  }
  $.toast({
    heading: "Update post",
    text: `Post ${post.title} has been updated by ${email}`,
    icon: "info",
    loader: true, // Change it to false to disable loader
    loaderBg: "#9EC600", // To change the background
  });
});

connection.on("DeletePost", function (post) {
  let postId = post.postID;
  if ($(`#post-${postId}`).length) {
    $(`#post-${postId}`).remove();
  }
});
