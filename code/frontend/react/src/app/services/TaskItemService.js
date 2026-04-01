// Detect environment - Docker serves on port 3000, local dev on port 1234
const API_BASE = typeof window !== 'undefined' && window.location.hostname === 'localhost'
  ? 'http://localhost:5148/api/taskitems'
  : 'http://backend:5148/api/taskitems';

export class TaskItemService {
  static buildUrl(path) {
    return API_BASE + path;
  }

  static async createTaskItem(request) {
    try {
      const response = await fetch(this.buildUrl(''), {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(request),
      });

      if (!response.ok) {
        const errorData = await response.json();
        throw new Error(errorData.error || 'Failed to create task item');
      }

      const data = await response.json();
      return data.data;
    } catch (error) {
      throw error;
    }
  }

  static async getTaskItem(id) {
    try {
      const response = await fetch(this.buildUrl('/' + id));

      if (!response.ok) {
        const errorData = await response.json();
        throw new Error(errorData.error || 'Failed to fetch task item');
      }

      const data = await response.json();
      return data.data;
    } catch (error) {
      throw error;
    }
  }

  static async getAllTaskItems() {
    try {
      const response = await fetch(this.buildUrl(''));

      if (!response.ok) {
        throw new Error('Failed to fetch task items');
      }

      const data = await response.json();
      return data.data || [];
    } catch (error) {
      return [];
    }
  }

  static async getTaskItemsByUserId(userId) {
    try {
      const url = this.buildUrl('/user/' + userId);
      const response = await fetch(url);

      if (!response.ok) {
        throw new Error('Failed to fetch user task items');
      }

      const data = await response.json();
      console.log('Tasks loaded:', data.data);
      return data.data || [];
    } catch (error) {
      console.warn('Task fetch failed:', error);
      return [];
    }
  }

  static async updateTaskItem(id, request) {
    try {
      const response = await fetch(this.buildUrl('/' + id), {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(request),
      });

      if (!response.ok) {
        const errorData = await response.json();
        throw new Error(errorData.error || 'Failed to update task item');
      }

      const data = await response.json();
      return data.data;
    } catch (error) {
      throw error;
    }
  }

  static async deleteTaskItem(id) {
    try {
      const response = await fetch(this.buildUrl('/' + id), {
        method: 'DELETE',
      });

      if (!response.ok) {
        const errorData = await response.json();
        throw new Error(errorData.error || 'Failed to delete task item');
      }

      return true;
    } catch (error) {
      throw error;
    }
  }
}
