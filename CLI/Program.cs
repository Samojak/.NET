using CLI.UI;
using CLI.UI.ManageComments;
using CLI.UI.ManagePosts;
using CLI.UI.ManageUsers;
using FileRepository;
using RepositoryContracts;

Console.WriteLine("Starting CLI app...");
IUserRepository userRepository = new UserFileRepository();
ICommentRepository commentRepository = new CommentFileRepository();
IPostRepository postRepository = new PostFileRepository();

CreateCommentView createCommentView = new CreateCommentView(commentRepository,userRepository);
CreatePostView  createPostView = new CreatePostView(postRepository, userRepository);
SinglePostView singlePostView = new SinglePostView(postRepository,commentRepository,userRepository,createCommentView);
ListPostView listPostView = new ListPostView(postRepository,userRepository,commentRepository,singlePostView);

CreateUserView  createUserView = new CreateUserView(userRepository);
ListUsersView listUsersView = new ListUsersView(userRepository);




CliApp cliApp = new CliApp(userRepository, commentRepository, postRepository, createCommentView, createPostView, listPostView, singlePostView,createUserView, listUsersView);
await cliApp.StartAsync();