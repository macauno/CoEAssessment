/**
 * TaskItem domain types matching the backend API
 * @typedef {Object} TaskItem
 * @property {number} id
 * @property {string} title
 * @property {string} description
 * @property {boolean} isCompleted
 * @property {string} createdAt
 * @property {string} userId
 */

/**
 * @typedef {Object} CreateTaskItemRequest
 * @property {string} title
 * @property {string} description
 * @property {string} userId
 */

/**
 * @typedef {Object} UpdateTaskItemRequest
 * @property {string} title
 * @property {string} description
 * @property {boolean} isCompleted
 */

/**
 * @typedef {Object} ApiResponse
 * @property {any} data
 * @property {string} [error]
 * @property {number} statusCode
 */

// Exports are handled naturally with JSDoc comments
