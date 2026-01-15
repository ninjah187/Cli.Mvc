# TODO

## appsettings.{Environment}.json handling

## async/await support

## Home / Index convention for running command ("")

## Implement Controller.Error method

- Add possibility to return non-zero app codes from controllers.
- Error should have following overloads: Error(string message, int code);

## Implement command pipeline

- Middlewares.
- Filters.
- `--help` option should work as a middleware.

## Add RazorLight views

- Make experimental preview version. Will be slow.
- Optimize later by using Razor.SDK and generating custom C# class based on .cshtml views.

