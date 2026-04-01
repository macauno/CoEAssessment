import { TaskItemService } from './TaskItemService';

describe('TaskItemService', () => {
  const mockResponse = (data, ok = true) => ({
    ok,
    json: async () => data,
  });

  beforeEach(() => {
    global.fetch = jest.fn();
  });

  afterEach(() => {
    jest.clearAllMocks();
  });

  describe('createTaskItem', () => {
    it('should create a task item successfully', async () => {
      const request = {
        title: 'Test Task',
        description: 'Test Description',
        userId: 'user123',
      };

      const mockData = {
        data: {
          id: 1,
          ...request,
          isCompleted: false,
          createdAt: '2026-04-01T00:00:00Z',
        },
        error: null,
        statusCode: 201,
      };

      global.fetch.mockResolvedValueOnce(mockResponse(mockData, true));

      const result = await TaskItemService.createTaskItem(request);

      expect(global.fetch).toHaveBeenCalledWith(
        'http://localhost:5148/api/taskitems',
        expect.objectContaining({
          method: 'POST',
          headers: { 'Content-Type': 'application/json' },
          body: JSON.stringify(request),
        })
      );
      expect(result).toEqual(mockData.data);
    });

    it('should throw error on creation failure', async () => {
      const request = {
        title: 'Test Task',
        description: 'Test Description',
        userId: 'user123',
      };

      const errorData = {
        error: 'Invalid input',
        statusCode: 400,
      };

      global.fetch.mockResolvedValueOnce(mockResponse(errorData, false));

      await expect(TaskItemService.createTaskItem(request)).rejects.toThrow(
        'Invalid input'
      );
    });
  });

  describe('getTaskItemsByUserId', () => {
    it('should fetch user tasks successfully', async () => {
      const userId = 'user123';
      const mockData = {
        data: [
          {
            id: 1,
            title: 'Buy Groceries',
            description: 'Get milk, eggs, and bread',
            isCompleted: false,
            createdAt: '2026-04-01T00:00:00Z',
            userId: userId,
          },
        ],
        error: null,
        statusCode: 200,
      };

      global.fetch.mockResolvedValueOnce(mockResponse(mockData, true));

      const result = await TaskItemService.getTaskItemsByUserId(userId);

      expect(global.fetch).toHaveBeenCalledWith(
        `http://localhost:5148/api/taskitems/user/${userId}`
      );
      expect(result).toEqual(mockData.data);
    });

    it('should return empty array on fetch failure', async () => {
      const userId = 'user123';
      global.fetch.mockResolvedValueOnce(mockResponse({}, false));

      const result = await TaskItemService.getTaskItemsByUserId(userId);

      expect(result).toEqual([]);
    });

    it('should return empty array if data is null', async () => {
      const userId = 'user123';
      const mockData = {
        data: null,
        error: null,
        statusCode: 200,
      };

      global.fetch.mockResolvedValueOnce(mockResponse(mockData, true));

      const result = await TaskItemService.getTaskItemsByUserId(userId);

      expect(result).toEqual([]);
    });
  });

  describe('getTaskItem', () => {
    it('should fetch a single task by id', async () => {
      const taskId = 1;
      const mockData = {
        data: {
          id: taskId,
          title: 'Test Task',
          description: 'Test Description',
          isCompleted: false,
          createdAt: '2026-04-01T00:00:00Z',
          userId: 'user123',
        },
        error: null,
        statusCode: 200,
      };

      global.fetch.mockResolvedValueOnce(mockResponse(mockData, true));

      const result = await TaskItemService.getTaskItem(taskId);

      expect(global.fetch).toHaveBeenCalledWith(
        `http://localhost:5148/api/taskitems/${taskId}`
      );
      expect(result).toEqual(mockData.data);
    });

    it('should throw error if task not found', async () => {
      const taskId = 999;
      const errorData = {
        error: 'Task not found',
        statusCode: 404,
      };

      global.fetch.mockResolvedValueOnce(mockResponse(errorData, false));

      await expect(TaskItemService.getTaskItem(taskId)).rejects.toThrow(
        'Task not found'
      );
    });
  });

  describe('updateTaskItem', () => {
    it('should update a task item successfully', async () => {
      const taskId = 1;
      const request = {
        title: 'Updated Task',
        description: 'Updated Description',
        isCompleted: true,
      };

      const mockData = {
        data: {
          id: taskId,
          ...request,
          createdAt: '2026-04-01T00:00:00Z',
          userId: 'user123',
        },
        error: null,
        statusCode: 200,
      };

      global.fetch.mockResolvedValueOnce(mockResponse(mockData, true));

      const result = await TaskItemService.updateTaskItem(taskId, request);

      expect(global.fetch).toHaveBeenCalledWith(
        `http://localhost:5148/api/taskitems/${taskId}`,
        expect.objectContaining({
          method: 'PUT',
          headers: { 'Content-Type': 'application/json' },
          body: JSON.stringify(request),
        })
      );
      expect(result).toEqual(mockData.data);
    });

    it('should throw error on update failure', async () => {
      const taskId = 1;
      const request = {
        title: 'Updated Task',
        description: 'Updated Description',
        isCompleted: true,
      };

      const errorData = {
        error: 'Task not found',
        statusCode: 404,
      };

      global.fetch.mockResolvedValueOnce(mockResponse(errorData, false));

      await expect(TaskItemService.updateTaskItem(taskId, request)).rejects.toThrow(
        'Task not found'
      );
    });
  });

  describe('deleteTaskItem', () => {
    it('should delete a task item successfully', async () => {
      const taskId = 1;
      const mockData = {
        data: true,
        error: null,
        statusCode: 204,
      };

      global.fetch.mockResolvedValueOnce(mockResponse(mockData, true));

      const result = await TaskItemService.deleteTaskItem(taskId);

      expect(global.fetch).toHaveBeenCalledWith(
        `http://localhost:5148/api/taskitems/${taskId}`,
        expect.objectContaining({
          method: 'DELETE',
        })
      );
      expect(result).toBe(true);
    });

    it('should throw error on delete failure', async () => {
      const taskId = 999;
      const errorData = {
        error: 'Task not found',
        statusCode: 404,
      };

      global.fetch.mockResolvedValueOnce(mockResponse(errorData, false));

      await expect(TaskItemService.deleteTaskItem(taskId)).rejects.toThrow(
        'Task not found'
      );
    });
  });

  describe('getAllTaskItems', () => {
    it('should fetch all task items', async () => {
      const mockData = {
        data: [
          {
            id: 1,
            title: 'Task 1',
            description: 'Description 1',
            isCompleted: false,
            createdAt: '2026-04-01T00:00:00Z',
            userId: 'user123',
          },
          {
            id: 2,
            title: 'Task 2',
            description: 'Description 2',
            isCompleted: true,
            createdAt: '2026-04-01T00:00:00Z',
            userId: 'user123',
          },
        ],
        error: null,
        statusCode: 200,
      };

      global.fetch.mockResolvedValueOnce(mockResponse(mockData, true));

      const result = await TaskItemService.getAllTaskItems();

      expect(global.fetch).toHaveBeenCalledWith(
        'http://localhost:5148/api/taskitems'
      );
      expect(result).toEqual(mockData.data);
    });

    it('should return empty array on fetch failure', async () => {
      global.fetch.mockResolvedValueOnce(mockResponse({}, false));

      const result = await TaskItemService.getAllTaskItems();

      expect(result).toEqual([]);
    });
  });
});
