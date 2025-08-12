import { Link } from "react-router-dom";

export const NotFoundPage = () => {
  return (
    <main className="flex flex-col items-center justify-center min-h-screen bg-gray-50 text-center px-6">
      <h1 className="text-6xl font-bold text-gray-800 mb-4">404</h1>
      <h2 className="text-2xl font-semibold text-gray-700 mb-2">
        Page Not Found
      </h2>
      <p className="text-gray-500 max-w-md mb-6">
        Sorry, the page you're looking for doesn’t exist or has been moved.
      </p>
      <Link
        to="/"
        className="px-6 py-3 bg-blue-600 text-white rounded-lg shadow hover:bg-blue-700 transition"
      >
        Go Back Home
      </Link>
    </main>
  );
};