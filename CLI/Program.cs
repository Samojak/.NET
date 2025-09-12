using CLI.UI;
using CLI.UI.Comments;
using CLI.UI.ManagePosts;
using CLI.UI.ManageUsers;
using InMemoryepositories;
using RepositoryContracts;

Console.WriteLine("Starting CLI app...");
IUserRepository userRepository = new UserInMemomoryRepository();
ICommentRepository commentRepository = new CommentInMemomoryRepository();
IPostRepository postRepository = new PostInMemoryRepository();

CreateCommentView createCommentView = new CreateCommentView(commentRepository);
CreatePostView  createPostView = new CreatePostView(postRepository, userRepository);
ListPostView listPostView = new ListPostView(postRepository);
SinglePostView singlePostView = new SinglePostView(postRepository);
CreateUserView  createUserView = new CreateUserView(userRepository);
ListUsersView listUsersView = new ListUsersView(userRepository);




CliApp cliApp = new CliApp(userRepository, commentRepository, postRepository, createCommentView, createPostView, listPostView, singlePostView,createUserView, listUsersView);
await cliApp.StartAsync();